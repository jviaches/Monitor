using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc;
using Monitor.Core.Models;
using Monitor.Core.Settings;
using ResourcesLambda.ViewModels;

namespace ResourcesLambda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private static AmazonDynamoDBClient client;
        private static DynamoDBContext context;

        public ResourcesController(Credentials credentials)
        {
            var awsOptions = new Amazon.Extensions.NETCore.Setup.AWSOptions()
            {
                Credentials = new BasicAWSCredentials(credentials.AccessKey, credentials.SecretKey),
                Region = RegionEndpoint.USEast2
            };

            client = new AmazonDynamoDBClient(awsOptions.Credentials, awsOptions.Region);
            context = new DynamoDBContext(client);
        }

        // GET: api/Resources/5
        [HttpGet("{id}", Name = "/Resources/Get")]
        public async Task<Resource> GetById(string id)
        {
            var item = await context.LoadAsync<Resource>(id);
            return item;
        }

        // GET: api/Resources/5
        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<IEnumerable<ResourceResultViewModel>> GetByUserId(string id)
        {
            var resourceConditions = new List<ScanCondition>
            {
               new ScanCondition("UserId", ScanOperator.Equal, id)
            };

            var allDocs = await context.ScanAsync<Resource>(resourceConditions).GetRemainingAsync();
            var resources = allDocs.Select(resource => new ResourceResultViewModel(resource)).ToArray();

            for (int i = 0; i < resources.Count(); i++)
            {
                var resourceHistoryConditions = new List<ScanCondition>
                {
                    new ScanCondition("ResourceId", ScanOperator.Equal, resources[i].Id)
                };

                var resourcesHistory = await context.ScanAsync<ResourcesHistory>(resourceHistoryConditions).GetRemainingAsync();
                var history = resourcesHistory.Select(resourcehistory => new ResourceHistoryResultViewModel(resourcehistory)).ToList();

                resources[i].history = history;
            }

            return resources;
        }

        // POST: api/Resources
        [HttpPost]
        public async Task Post([FromBody] ResourceViewModel resourceHistoryVM)
        {
            var periodTimeInSeconds = resourceHistoryVM.MonitorPeriod * 1000;
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
        }

        // PUT: api/Resources/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
