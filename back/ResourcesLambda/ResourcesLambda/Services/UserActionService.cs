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
using Monitor.Core.ViewModels.UserActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcesLambda.Services
{
    public class UserActionService : IUserActionService
    {
        private static AmazonDynamoDBClient client;
        private static DynamoDBContext context;
        
        private const string TABLE_NAME = "UserActions";

        public UserActionService(Credentials credentials)
        {
            var awsOptions = new Amazon.Extensions.NETCore.Setup.AWSOptions()
            {
                Credentials = new BasicAWSCredentials(credentials.AccessKey, credentials.SecretKey),
                Region = RegionEndpoint.USEast2
            };

            client = new AmazonDynamoDBClient(awsOptions.Credentials, awsOptions.Region);
            context = new DynamoDBContext(client);
        }

        public async Task<Result> Add(UserActionViewModel userActionVM)
        {
            try
            {
                var putItemRequest = new PutItemRequest()
                {
                    TableName = TABLE_NAME,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        {"Id", new AttributeValue {S = Guid.NewGuid().ToString()}},
                        {"UserId", new AttributeValue {S = userActionVM.UserId}},
                        {"Date", new AttributeValue {S = userActionVM.Date}},
                        {"Action", new AttributeValue {S = userActionVM.Action}},
                        {"Data", new AttributeValue {S = userActionVM.Data}},
                    }
                };

                await client.PutItemAsync(putItemRequest);
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }
    }
}
