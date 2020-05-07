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
    public class ResourceHistoryService : IResourceHistoryService
    {
        private IResourceHistoryRepository _resourceHistoryRepository;

        public ResourceHistoryService(IResourceHistoryRepository resourceHistoryRepository)
        {
            _resourceHistoryRepository = resourceHistoryRepository;
        }

        public ResourcesHistory Add(AddResourceHistoryDto resourcedto)
        {
            var resourceHistory = new ResourcesHistory()
            {
                RequestDate = resourcedto.RequestDate,
                ResourceId = resourcedto.ResourceId,
                Result = resourcedto.Result
            };

            return _resourceHistoryRepository.Add(resourceHistory);
        }

        public Task<ResourcesHistory> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResourcesHistory> ResourceId(int resourceId)
        {
            throw new NotImplementedException();
        }
    }
}
