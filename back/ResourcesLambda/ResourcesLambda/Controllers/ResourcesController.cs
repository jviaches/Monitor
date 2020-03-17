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
using ResourcesLambda.Models;
using ResourcesLambda.Settings;
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
        public async Task<IEnumerable<Resource>> GetByUserId(string id)
        {
            var conditions = new List<ScanCondition>
            {
               new ScanCondition("userId", ScanOperator.Equal, id)
            };

            var allDocs = await context.ScanAsync<Resource>(conditions).GetRemainingAsync();
            return allDocs;
        }

        // POST: api/Resources
        [HttpPost]
        public async Task Post([FromBody] ResourceViewModel resourceHistoryVM)
        {
            var putItemRequest = new PutItemRequest()
            {
                TableName = "Resources",
                Item = new Dictionary<string, AttributeValue>
                {
                    {"id", new AttributeValue {S = Guid.NewGuid().ToString()}},
                    {"isMonitorActivated", new AttributeValue {BOOL = resourceHistoryVM.isMonitorActivated}},
                    {"monitorActivationDate", new AttributeValue {S = resourceHistoryVM.monitorActivationDate.ToString()}},
                    {"monitorActivationType", new AttributeValue {S = resourceHistoryVM.monitorActivationType.ToString()}},
                    {"url", new AttributeValue {S = resourceHistoryVM.url.ToString()}},
                    {"userId", new AttributeValue {S = resourceHistoryVM.userId.ToString()}},
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
