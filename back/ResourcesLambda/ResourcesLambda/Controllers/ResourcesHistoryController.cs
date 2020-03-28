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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Monitor.Core.Models;
using Monitor.Core.Settings;
using ResourcesLambda.ViewModels;

namespace ResourcesLambda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesHistoryController : ControllerBase
    {
        private static AmazonDynamoDBClient client;
        private static DynamoDBContext context;

        public ResourcesHistoryController(Credentials credentials)
        {
            var awsOptions = new Amazon.Extensions.NETCore.Setup.AWSOptions()
            {
                Credentials = new BasicAWSCredentials(credentials.AccessKey, credentials.SecretKey),
                Region = RegionEndpoint.USEast2
            };

            client = new AmazonDynamoDBClient(awsOptions.Credentials, awsOptions.Region);
            context = new DynamoDBContext(client);
        }

        // GET: api/ResourcesHistory/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ResourcesHistory> Get(string id)
        {
           var item = await context.LoadAsync<ResourcesHistory>(id);
            return item;
        }

        // GET: api/Resources/5
        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<IEnumerable<ResourcesHistory>> GetByResourceId(string id)
        {
            var conditions = new List<ScanCondition>
            {
               new ScanCondition("resourceId", ScanOperator.Equal, id)
            };

            var allDocs = await context.ScanAsync<ResourcesHistory>(conditions).GetRemainingAsync();
            return allDocs;
        }

        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<IEnumerable<ResourcesHistory>> GetByMonitorTypeId(string id)
        {
            var conditions = new List<ScanCondition>
            {
               new ScanCondition("monitorTypeId", ScanOperator.Equal, id)
            };

            var allDocs = await context.ScanAsync<ResourcesHistory>(conditions).GetRemainingAsync();
            return allDocs;
        }

        // POST: api/ResourcesHistory
        [HttpPost]
        public async Task Post([FromBody] ResourceHistoryViewModel resourceHistoryVM)
        {
            var putItemRequest = new PutItemRequest()
            {
                TableName = "ResourcesHistory",
                Item = new Dictionary<string, AttributeValue>
                {
                    {"id", new AttributeValue {S = Guid.NewGuid().ToString()}},
                    {"resourceId", new AttributeValue {S = resourceHistoryVM.resourceId.ToString()}},
                    {"monitorTypeId", new AttributeValue {S = resourceHistoryVM.monitorTypeId.ToString()}},
                    {"requestDate", new AttributeValue {S = resourceHistoryVM.requestDate.ToString()}},
                }
            };

            await client.PutItemAsync(putItemRequest);
        }

        // PUT: api/ResourcesHistory/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE: api/ResourcesHistory/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
