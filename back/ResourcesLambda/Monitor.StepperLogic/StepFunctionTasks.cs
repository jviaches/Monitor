using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Lambda;
using Amazon.Lambda.Core;
using Amazon.Lambda.Model;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Monitor.Core.Models;
using Monitor.StepperLogic.StateModels;
using Newtonsoft.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Monitor.StepperLogic
{
    public class StepFunctionTasks
    {
        private static AmazonDynamoDBClient dbClient;
        private static DynamoDBContext dbContext;
        private static AmazonLambdaClient lambdaClient;

        public StepFunctionTasks()
        {
            var awsOptions = new AWSOptions()
            {
                Credentials = new BasicAWSCredentials("AKIATCF5UENNOJWDLUVU", "YUy4AL06ZBfHpNuRWn2F2TE04vJ/UUeZz+fmN5+i"),
                Region = RegionEndpoint.USEast2
            };

            lambdaClient = new AmazonLambdaClient(awsOptions.Credentials, awsOptions.Region);
            dbClient = new AmazonDynamoDBClient(awsOptions.Credentials, awsOptions.Region);
            dbContext = new DynamoDBContext(dbClient);
        }

        /// <summary>
        /// Lamda function responsible to retrieve web-sites stored in DB by specified periodicity
        /// </summary>
        /// <param name="model">Periodicity</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<ResourceScanResultModel> RetrieveRecords(RetrieveRecordModel model, ILambdaContext context)
        {
            context.Logger.LogLine($"---------- RetrieveRecords  from DB with Periodicity [{(model.Periodicity)} milisec] ----------");

            var resourceConditions = new List<ScanCondition>
            {
               new ScanCondition("MonitorPeriod", ScanOperator.Equal, model.Periodicity),
               new ScanCondition("IsMonitorActivated", ScanOperator.Equal, 1)
            };

            var resources = await dbContext.ScanAsync<Resource>(resourceConditions).GetRemainingAsync();
            var urlList = resources.Select(record => new ResourceModel(record.Id, record.Url, string.Empty)).ToList();

            var resourceToScanModel = new ResourceScanResultModel() { ResourcesStatuses = urlList };

            context.Logger.LogLine($"Retrieved {resourceToScanModel.ResourcesStatuses.Count} resources for scan with specified periodicity");

            return resourceToScanModel;
        }

        public async Task<ResourceScanResultModel> MonitorRecords(ResourceScanResultModel model, ILambdaContext context)
        {
            context.Logger.LogLine($"---------- Start MonitorRecords [{model.ResourcesStatuses.Count} resources] ----------");
            var resourceScanResultModel = new ResourceScanResultModel();

            foreach (var item in model.ResourcesStatuses)
            {
                var lamdaRequest = new InvokeRequest
                {
                    FunctionName = "MonitorStepperLogic-GetResourceStatusTask-P53XRWMQLD3Z",
                    InvocationType = Amazon.Lambda.InvocationType.RequestResponse,
                    Payload = JsonConvert.SerializeObject(item.Url)
                };

                try
                {
                    var lamdaResponse = await lambdaClient.InvokeAsync(lamdaRequest);
                    var stReader = new StreamReader(lamdaResponse.Payload);

                    var jReader = new JsonTextReader(stReader);

                    var jSerializer = new JsonSerializer();
                    var statusCode = jSerializer.Deserialize(jReader);

                    resourceScanResultModel.ResourcesStatuses.Add(new ResourceModel(item.ResourceId, item.Url, statusCode.ToString()));
                }
                catch (Exception e)
                {
                    context.Logger.LogLine($"Exception Occured: {e}");
                    resourceScanResultModel.ResourcesStatuses.Add(new ResourceModel(item.ResourceId, item.Url, "ERR"));
                }
            }

            context.Logger.LogLine($"---------- End MonitorRecords ----------");

            return resourceScanResultModel;
        }

        public async Task<ResourceScanResultModel> ProcessRecords(ResourceScanResultModel model, ILambdaContext context)
        {
            context.Logger.LogLine($"---------- Start ProcessRecords ----------");

            //TODO: make 1 DB batch operation
            //TODO: handle exception

            foreach (var item in model.ResourcesStatuses)
            {
                var putItemRequest = new PutItemRequest()
                {
                    TableName = "ResourcesHistory",
                    Item = new Dictionary<string, AttributeValue>
                    {
                        {"Id", new AttributeValue {S = Guid.NewGuid().ToString()}},
                        {"ResourceId", new AttributeValue {S = item.ResourceId}},
                        {"RequestDate", new AttributeValue {S = DateTime.UtcNow.ToString()}},
                        {"Result", new AttributeValue {S = item.StatusCode}}
                    }
                };

                await dbClient.PutItemAsync(putItemRequest);
            }

            context.Logger.LogLine($"---------- End ProcessRecords ----------");

            return model;
        }

        public async Task<string> GetResourceStatus(string url, ILambdaContext context)
        {
            //TODO: improve by checking existing URL + Status code
            //TODO: on exception => send email to customer email

            context.Logger.LogLine($"---------- GetResourceStatus Invocation: [{url}] ----------");

            try
            {
                var client = new HttpClient();
                var result = await client.GetAsync(url);
                context.Logger.LogLine($"Status code for URL: [{url}] = [{(int)result.StatusCode}]");

                return ((int)result.StatusCode).ToString();
            }
            catch (Exception e)
            {
                context.Logger.LogLine($"Exception occured: [{e}] ");
                await sendEmail(url, context);
                return string.Empty;
            }
        }

        private async Task sendEmail(string URL, ILambdaContext context)
        {
            const string senderAddress = "support@projscope.com";

            // Replace recipient@example.com with a "To" address. If your account
            // is still in the sandbox, this address must be verified.
            const string receiverAddress = "jviaches@gmail.com";

            const string subject = "Critical Alert";
            const string textBody = "Amazon SES Test (.NET)\r\n"
                                            + "This email was sent through Amazon SES "
                                            + "using the AWS SDK for .NET.";

            // The HTML body of the email.
            const string htmlBody = @"<html>
            <head></head>
            <body>
              <h1>Amazon SES Test (AWS SDK for .NET)</h1>
              <p>This email was sent with
                <a href='https://aws.amazon.com/ses/'>Amazon SES</a> using the
                <a href='https://aws.amazon.com/sdk-for-net/'>
                  AWS SDK for .NET</a>.</p>
            </body>
            </html>";

            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses =
                        new List<string> { receiverAddress }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = htmlBody
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = textBody
                            }
                        }
                    },
                };
                try
                {
                    context.Logger.LogLine($"Sending email using Amazon SES...");
                    var response = await client.SendEmailAsync(sendRequest);
                    context.Logger.LogLine($"The email was sent successfully.");
                }
                catch (Exception ex)
                {
                    context.Logger.LogLine($"The email was not sent ");
                    context.Logger.LogLine($"Exception occured: [{ex.Message}] ");
                }
            }
        }
    }
}