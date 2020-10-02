using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Lambda;
using Amazon.Lambda.Core;
using Amazon.Lambda.Model;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Monitor.Core.Dto;
using Monitor.Infra;
using Monitor.Infra.Entities;
using Monitor.Infra.Interfaces.Repository;
using Monitor.Infra.Repositories;
using Monitor.Infra.Services;
using Monitor.StepperLogic.StateModels;
using Newtonsoft.Json;
using Environment = System.Environment;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Monitor.StepperLogic
{
    public class StepFunctionTasks
    {
        private static AmazonLambdaClient lambdaClient;
        private IResourceService _resourceService;
        private IResourceHistoryService _resourceHistoryService;

        public StepFunctionTasks()
        {
            var awsOptions = new AWSOptions()
            {
                Credentials = new BasicAWSCredentials("AKIATCF5UENNOJWDLUVU", "YUy4AL06ZBfHpNuRWn2F2TE04vJ/UUeZz+fmN5+i"),
                Region = RegionEndpoint.USEast2
            };

            lambdaClient = new AmazonLambdaClient(awsOptions.Credentials, awsOptions.Region);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceRepository, ResourceRepository>();
            serviceCollection.AddSingleton<IResourceHistoryRepository, ResourceHistoryRepository>();

            serviceCollection.AddSingleton<IResourceService, ResourceService>();
            serviceCollection.AddSingleton<IResourceHistoryService, ResourceHistoryService>();
            serviceCollection.AddSingleton<IUserActionService, UserActionService>();
            serviceCollection.AddSingleton<IUserActionRepository, UserActionRepository>();

            var DBHostName = Environment.GetEnvironmentVariable("DBHostName");
            var DBName = Environment.GetEnvironmentVariable("DBName");
            var DBUserName = Environment.GetEnvironmentVariable("DBUserName");
            var DBPassword = Environment.GetEnvironmentVariable("DBPassword");
            var DBPort = Environment.GetEnvironmentVariable("DBPort");

            serviceCollection.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql($"Host={DBHostName};Port={DBPort};Username={DBUserName};Password={DBPassword};Database={DBName};", b => b.MigrationsAssembly("Monitor.Infra"));
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _resourceService = serviceProvider.GetService<IResourceService>();
            _resourceHistoryService = serviceProvider.GetService<IResourceHistoryService>();
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

            var resources = await _resourceService.GetByPeriodicityAndMonitor(model.Periodicity, true);

            var urlList = resources.Select(record => new ResourceModel(record.Id, record.Url, string.Empty, record.UserId.ToString())).ToList();

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
                    //Todo: call lamda depending on environment
                    FunctionName = "ResourceStepperStaging-GetResourceStatusTask-WKMVBYB2Q548",
                    InvocationType = Amazon.Lambda.InvocationType.RequestResponse,
                    Payload = JsonConvert.SerializeObject(item)
                };

                try
                {
                    var lamdaResponse = await lambdaClient.InvokeAsync(lamdaRequest);
                    var stReader = new StreamReader(lamdaResponse.Payload);

                    var jReader = new JsonTextReader(stReader);

                    var jSerializer = new JsonSerializer();
                    var statusCode = jSerializer.Deserialize(jReader);

                    resourceScanResultModel.ResourcesStatuses.Add(new ResourceModel(item.ResourceId, item.Url, statusCode.ToString(), item.OwnerId));
                }
                catch (Exception e)
                {
                    context.Logger.LogLine($"Exception Occured: {e}");
                    resourceScanResultModel.ResourcesStatuses.Add(new ResourceModel(item.ResourceId, item.Url, "000", item.OwnerId));
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
                var addResourceHistoryDto = new AddResourceHistoryDto()
                {
                    ResourceId = item.ResourceId,
                    RequestDate = DateTime.UtcNow,
                    Result = string.IsNullOrEmpty(item.StatusCode) ? "000" : item.StatusCode
                };

                _resourceHistoryService.Add(addResourceHistoryDto);
            }

            context.Logger.LogLine($"---------- End ProcessRecords ----------");

            return model;
        }

        public async Task<string> GetResourceStatus(ResourceModel resource, ILambdaContext context)
        {
            //TODO: improve by checking existing URL + Status code
            context.Logger.LogLine($"---------- GetResourceStatus Invocation: [{resource.Url}] ----------");

            try
            {
                var client = new HttpClient();
                var result = await client.GetAsync(resource.Url);
                var statusCodeResult = ((int)result.StatusCode).ToString();
                context.Logger.LogLine($"Status code for URL: [{resource.Url}] = [{statusCodeResult}]");

                if (statusCodeResult.StartsWith("3") || statusCodeResult.StartsWith("4") || statusCodeResult.StartsWith("5"))
                    await sendEmail(resource, statusCodeResult, context);

                return statusCodeResult;
            }
            catch (Exception e)
            {
                context.Logger.LogLine($"Exception occured: [{e}] ");
                await sendEmail(resource, "ERROR", context); //TODO: send to support@projscope.com
                return string.Empty;
            }
        }

        private async Task sendEmail(ResourceModel resource, string statusCodeResult, ILambdaContext context)
        {
            context.Logger.LogLine($"---------- Recieved input: {resource} ----------");

            var cognitoidentityserviceprovider = new AmazonCognitoIdentityProviderClient();
            var resourceOwner = await cognitoidentityserviceprovider.AdminGetUserAsync(
                                new AdminGetUserRequest() { Username = resource.OwnerId, UserPoolId = "us-east-2_AzTuf9Pg2" });

            var ownerEmail = resourceOwner.UserAttributes[2].Value; // email
            context.Logger.LogLine($"resource owner found: {ownerEmail}");

            const string senderAddress = "support@projscope.com";

            // Replace recipient@example.com with a "To" address. If your account
            // is still in the sandbox, this address must be verified.
            string receiverAddress = ownerEmail; 

            const string subject = "Projscope - Alert";
            string textBody = "Abnormal resource status detected - " + statusCodeResult + "\r\n"
                                            + "Status code: " + resource + ".";

            // The HTML body of the email.
            string htmlBody = string.Format(@"<html>
            <head></head>
            <body>
                <h1>Projscope - Alert</h1>
                <div>Abnormal resource status detected: {0}</ div>
                <div>Site status code: {1}</ div>
                <p>This email was sent by
                    <a href='https://projscope.com/'>Proscope.com</a>.
            </body>
            </html>", resource.Url, statusCodeResult);

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
