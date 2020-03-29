using System;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.DynamoDBv2.Model;
using Monitor.Core.Models;
using System.Net;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Amazon;
using Amazon.Runtime;
using Amazon.DynamoDBv2;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ScanResources
{
    public class Function
    {
        // private static readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        public async Task FunctionHandler(ILambdaContext context)
        {
            context.Logger.LogLine($"START");
            context.Logger.LogLine($"input received");

            var awsOptions = new Amazon.Extensions.NETCore.Setup.AWSOptions()
            {

                Credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("AccessKey"),
                                                      Environment.GetEnvironmentVariable("SecretKey")),
                Region = RegionEndpoint.USEast2
            };

            var client = new AmazonDynamoDBClient(awsOptions.Credentials, awsOptions.Region);
            var dbContext = new DynamoDBContext(client);

            string id = "0ad50ebe-f19c-4339-9578-e6a539e216cd"; //temporary
            try
            {
                var item = dbContext.LoadAsync<Resource>(id);
                Uri uri = new Uri(item.Result.url);
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                var statusCode = (int)myHttpWebResponse.StatusCode;

                myHttpWebResponse.Close();

                var putItemRequest = new PutItemRequest()
                {
                    TableName = "ResourcesHistory",
                    Item = new Dictionary<string, AttributeValue>
                    {
                        {"id", new AttributeValue {S = Guid.NewGuid().ToString()}},
                        {"resourceId", new AttributeValue {S = id}},
                        {"monitorTypeId", new AttributeValue {S = 1.ToString()}},
                        {"requestDate", new AttributeValue {S = DateTime.UtcNow.ToString()}},
                        {"responseDate", new AttributeValue {S = DateTime.UtcNow.ToString()}},
                        {"result", new AttributeValue {S = statusCode.ToString()}},
                    }
                };

                await client.PutItemAsync(putItemRequest);
            }
            catch (WebException e)
            {
                context.Logger.LogLine("Web exception: " + e);
            }
            catch (Exception e)
            {
                context.Logger.LogLine("Exception: " + e);
            }

            context.Logger.LogLine("Stream processing complete.");
        }
    }
}