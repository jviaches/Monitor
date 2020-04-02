using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Monitor.Core;
using Monitor.Core.Interfaces;
using Monitor.Core.Models;
using Monitor.Core.Settings;
using Monitor.Core.Validations;
using Monitor.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcesLambda.Services
{
    public class ResourceService : IResourceService
    {
        private static AmazonDynamoDBClient client;
        private static DynamoDBContext context;

        public ResourceService(Credentials credentials)
        {
            var awsOptions = new Amazon.Extensions.NETCore.Setup.AWSOptions()
            {
                Credentials = new BasicAWSCredentials(credentials.AccessKey, credentials.SecretKey),
                Region = RegionEndpoint.USEast2
            };

            client = new AmazonDynamoDBClient(awsOptions.Credentials, awsOptions.Region);
            context = new DynamoDBContext(client);
        }
        public async Task<Resource> GetById(string id)
        {
            var item = await context.LoadAsync<Resource>(id);
            return item;
        }

        public async Task<IEnumerable<Resource>> GetByUserId(string id)
        {
            var resourceConditions = new List<ScanCondition>
            {
               new ScanCondition("UserId", ScanOperator.Equal, id)
            };

            var resources = await context.ScanAsync<Resource>(resourceConditions).GetRemainingAsync();
            for (int i = 0; i < resources.Count(); i++)
            {
               var resourceHistoryConditions = new List<ScanCondition>
               {
                   new ScanCondition("ResourceId", ScanOperator.Equal, resources[i].Id)
               };

                var resourcesHistory = await context.ScanAsync<ResourcesHistory>(resourceHistoryConditions).GetRemainingAsync();
                resources[i].History = new List<ResourcesHistory>(resourcesHistory);
            }

            return resources;
        }

        public async Task<Result> Add(ResourceViewModel resourceHistoryVM)
        {
            try
            {
                var periodTimeInSeconds = resourceHistoryVM.MonitorPeriod;
                var putItemRequest = new PutItemRequest()
                {
                    TableName = "Resources",
                    Item = new Dictionary<string, AttributeValue>
                    {
                        {"Id", new AttributeValue {S = Guid.NewGuid().ToString()}},
                        {"Url", new AttributeValue {S = resourceHistoryVM.Url}},
                        {"UserId", new AttributeValue {S = resourceHistoryVM.UserId}},
                        {"MonitorPeriod", new AttributeValue {N = periodTimeInSeconds.ToString()}},
                        {"IsMonitorActivated", new AttributeValue {N = resourceHistoryVM.IsMonitorActivated.ToString()}},
                        {"MonitorActivationDate", new AttributeValue {S = resourceHistoryVM.MonitorActivationDate}},
                    }
                };

                await client.PutItemAsync(putItemRequest);
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail();
            }
        }

        public async Task<Result> Update(UpdateResourceViewModel resourceVM)
        {
            try
            {
                var periodTimeInSeconds = resourceVM.MonitorPeriod;
                var updateItemRequest = new UpdateItemRequest()
                {
                    TableName = "Resources",
                    Key = new Dictionary<string, AttributeValue> { { "Id", new AttributeValue { S = resourceVM.Id } } },
                    ExpressionAttributeNames = new Dictionary<string, string>()
                    {
                        {"#MP", "MonitorPeriod"},
                        {"#MA", "IsMonitorActivated"},
                    },

                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                    {
                        {":mp",new AttributeValue {N = resourceVM.MonitorPeriod.ToString()}},
                        {":ma",new AttributeValue {N = resourceVM.IsMonitorActivated.ToString()}}
                    },

                    UpdateExpression = "SET #MP = :mp, #MA = :ma"
                };

                await client.UpdateItemAsync(updateItemRequest);
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail();
            }
        }

        public async Task<Result> Delete(UpdateResourceViewModel resourceVM)
        {
            try
            {
                // delete resource history related entries
                var historyItems = await GetHistoryByResourceId(resourceVM.Id);

                var deleteHistoryBatch = context.CreateBatchWrite<ResourcesHistory>();
                deleteHistoryBatch.AddDeleteItems(historyItems);
                await deleteHistoryBatch.ExecuteAsync();

                // delete resource entry
                string tableName = "Resources";

                var request = new DeleteItemRequest
                {
                    TableName = tableName,
                    Key = new Dictionary<string, AttributeValue>() { { "Id", new AttributeValue { S = resourceVM.Id } } },
                };

                await client.DeleteItemAsync(request);
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail();
            }
        }

        public async Task<IEnumerable<ResourcesHistory>> GetHistoryByResourceId(string resourceId)
        {
            var resourceHistoryConditions = new List<ScanCondition>
            {
               new ScanCondition("ResourceId", ScanOperator.Equal,resourceId)
            };

            var resourcesHistory = await context.ScanAsync<ResourcesHistory>(resourceHistoryConditions).GetRemainingAsync();
            return resourcesHistory;
        }
    }
}
