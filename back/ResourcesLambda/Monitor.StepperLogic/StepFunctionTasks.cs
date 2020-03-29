using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
            context.Logger.LogLine($"---------- RetrieveRecords  from DB with Periodicity [{(model.Periodicity * 1000)} milisec] ----------");

            //TODO: retrieve monitor activate only (i.e. isMonitorActivated=true)

            var resourceConditions = new List<ScanCondition>
            {
               new ScanCondition("MonitorPeriod", ScanOperator.Equal, (model.Periodicity * 1000)),
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
                //TODO: set limit to monitor under 1k sites + paging
                var lamdaRequest = new InvokeRequest
                {
                    FunctionName = "MonitorStepperLogic-GetResourceStatusTask-P53XRWMQLD3Z",
                    InvocationType = InvocationType.RequestResponse,
                    Payload = JsonConvert.SerializeObject(item.Url)
                };

                var lamdaResponse = await lambdaClient.InvokeAsync(lamdaRequest);
                var stReader = new StreamReader(lamdaResponse.Payload);
                
                var jReader = new JsonTextReader(stReader);
                
                var jSerializer = new JsonSerializer();
                var statusCode = jSerializer.Deserialize(jReader);

                resourceScanResultModel.ResourcesStatuses.Add(new ResourceModel(item.ResourceId, item.Url, statusCode.ToString()));
            }

            context.Logger.LogLine($"---------- End MonitorRecords ----------");

            return resourceScanResultModel;
        }

        public async Task<ResourceScanResultModel> ProcessRecords(ResourceScanResultModel model, ILambdaContext context)
        {
            context.Logger.LogLine($"---------- Start ProcessRecords ----------");

            //TODO: how to make 1 DB write operation instead of many ?
            //TODO: handle exception
            //TODO: on exception => send email to customer email

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

            context.Logger.LogLine($"---------- GetResourceStatus Invocation: [{url}] ----------");
            var client = new HttpClient();
            var result = await client.GetAsync(url);
            return ((int)result.StatusCode).ToString();
        }
    }
}
