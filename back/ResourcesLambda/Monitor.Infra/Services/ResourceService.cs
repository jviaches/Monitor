using Monitor.Core.Dto;
using Monitor.Core.Settings;
using Monitor.Infra.Entities;
using Monitor.Infra.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Infra.Services
{
    public class ResourceService : IResourceService
    {
        private IResourceRepository _resourceRepository;

        public ResourceService(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }
        public async Task<Resource> GetById(int id)
        {
            return await _resourceRepository.GetById(id);
        }

        public async Task<IEnumerable<Resource>> GetByUserId(Guid userId)
        {
            return await _resourceRepository.GetByUserId(userId);
        }

        public Resource Add(AddResourceDto resourcedto)
        {
            Resource resource = new Resource()
            {
                IsMonitorActivated = resourcedto.IsMonitorActivated,
                MonitorPeriod = resourcedto.MonitorPeriod,
                Url = resourcedto.Url,
                UserId = resourcedto.UserId,
                MonitorActivationDate = DateTime.UtcNow
            };

            return _resourceRepository.Add(resource);
        }

        public async Task<IEnumerable<Resource>> GetByPeriodicityAndMonitor(int periodicity, bool isMonitored)
        {
            return await _resourceRepository.GetByPeriodicityAndMonitor(periodicity, isMonitored);
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
    }
}
