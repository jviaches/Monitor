using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.EntityFrameworkCore;
using Monitor.Core;
using Monitor.Core.Interfaces;
using Monitor.Core.Models;
using Monitor.Core.Settings;
using Monitor.Core.Validations;
using Monitor.Core.ViewModels;
using Monitor.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcesLambda.Services
{
    public class ResourceService : IResourceService
    {
        private AppDbContext _dbContext;

        public ResourceService(Credentials credentials, AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<Resource> GetById(int id)
        {
            var resource = _dbContext.Resources
                   .Include(res => res.History)
                   .SingleOrDefaultAsync(res => res.Id == id);

            return await resource;
        }

        public async Task<IEnumerable<Resource>> GetByUserId(int userId)
        {
            var resources = _dbContext.Resources
                   .Include(res => res.History)
                   .Where(res => res.UserId == userId);

            return await resources.ToListAsync();
        }

        public Resource Add(ResourceViewModel resourceHistoryVM)
        {
            Resource res = new Resource()
            {
                IsMonitorActivated = resourceHistoryVM.IsMonitorActivated,
                MonitorPeriod = resourceHistoryVM.MonitorPeriod,
                Url = resourceHistoryVM.Url,
                UserId = resourceHistoryVM.UserId,
                MonitorActivationDate = DateTime.UtcNow
            };

            _dbContext.Set<Resource>().Add(res);
            _dbContext.SaveChanges();

            return res;
            }
        }

        //public async Task<Result> Update(UpdateResourceViewModel resourceVM)
        //{
        //    try
        //    {
        //        var periodTimeInSeconds = resourceVM.MonitorPeriod;
        //        var updateItemRequest = new UpdateItemRequest()
        //        {
        //            TableName = "Resources-prod",
        //            Key = new Dictionary<string, AttributeValue> { { "Id", new AttributeValue { S = resourceVM.Id } } },
        //            ExpressionAttributeNames = new Dictionary<string, string>()
        //            {
        //                {"#MP", "MonitorPeriod"},
        //                {"#MA", "IsMonitorActivated"},
        //            },

        //            ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
        //            {
        //                {":mp",new AttributeValue {N = resourceVM.MonitorPeriod.ToString()}},
        //                {":ma",new AttributeValue {N = resourceVM.IsMonitorActivated.ToString()}}
        //            },

        //            UpdateExpression = "SET #MP = :mp, #MA = :ma"
        //        };

        //        await client.UpdateItemAsync(updateItemRequest);
        //        return Result.Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return Result.Fail();
        //    }
        //}

        //public async Task<Result> Delete(UpdateResourceViewModel resourceVM)
        //{
        //    try
        //    {
        //        // delete resource history related entries
        //        var historyItems = await GetHistoryByResourceId(resourceVM.Id);

        //        var deleteHistoryBatch = context.CreateBatchWrite<ResourcesHistory>();
        //        deleteHistoryBatch.AddDeleteItems(historyItems);
        //        await deleteHistoryBatch.ExecuteAsync();

        //        // delete resource entry
        //        string tableName = "Resources";

        //        var request = new DeleteItemRequest
        //        {
        //            TableName = tableName,
        //            Key = new Dictionary<string, AttributeValue>() { { "Id", new AttributeValue { S = resourceVM.Id } } },
        //        };

        //        await client.DeleteItemAsync(request);
        //        return Result.Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return Result.Fail();
        //    }
        //}

        //public async Task<IEnumerable<ResourcesHistory>> GetHistoryByResourceId(string resourceId)
        //{
        //    var resourceHistoryConditions = new List<ScanCondition>
        //    {
        //       new ScanCondition("ResourceId", ScanOperator.Equal,resourceId)
        //    };

        //    var resourcesHistory = await context.ScanAsync<ResourcesHistory>(resourceHistoryConditions).GetRemainingAsync();
        //    return resourcesHistory;
        //}
    //}
}
