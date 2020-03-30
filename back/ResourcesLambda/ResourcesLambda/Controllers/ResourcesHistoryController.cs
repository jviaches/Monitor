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

        // GET: api/Resources/5
        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<IEnumerable<ResourcesHistory>> GetByResourceId(string id)
        {
            var conditions = new List<ScanCondition>
            {
               new ScanCondition("ResourceId", ScanOperator.Equal, id)
            };

            var allDocs = await context.ScanAsync<ResourcesHistory>(conditions).GetRemainingAsync();
            return allDocs;
        }

        //[Route("[action]/{id}")]
        //[HttpGet]
        //public async Task<IEnumerable<ResourcesHistory>> GetByMonitorTypeId(string id)
        //{
        //    var conditions = new List<ScanCondition>
        //    {
        //       new ScanCondition("monitorTypeId", ScanOperator.Equal, id)
        //    };

        //    var allDocs = await context.ScanAsync<ResourcesHistory>(conditions).GetRemainingAsync();
        //    return allDocs;
        //}
    }
}
