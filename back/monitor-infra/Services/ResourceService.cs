using monitor_core.Dto;
using monitor_core.Enums;
using monitor_infra.Entities;
using monitor_infra.Repositories.Interfaces;
using monitor_infra.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using System.Threading.Tasks;
using IResourceService = monitor_infra.Services.Interfaces.IResourceService;

namespace monitor_infra.Services
{
    public class ResourceService : IResourceService
    {
        private IResourceRepository _resourceRepository;
        private readonly IUserActionService _userActionService;

        public ResourceService(IResourceRepository resourceRepository, IUserActionService userActionService)
        {
            _resourceRepository = resourceRepository;
            _userActionService = userActionService;
        }
        public async Task<Resource> GetById(Guid id)
        {
            return await _resourceRepository.GetById(id);
        }

        public async Task<IEnumerable<Resource>> GetByUserId(int userId)
        {
            return await _resourceRepository.GetByUserId(userId);
        }

        public Resource Add(AddResourceDto resourcedto)
        {
            // TODO: throw domain event
            _userActionService.Add(new AddUserActionDto()
            {
                UserId = resourcedto.UserId,
                Date = DateTime.UtcNow,
                Action = UserActiontype.ResourceAdded,
                Data = $@"Url: [{resourcedto.Url}], 
                       MonitorActivated: [{resourcedto.MonitorActivated}]"
            });

            return _resourceRepository.Add(resourcedto);
        }

        public async Task Delete(Guid resourceId)
        {
            var resource = await _resourceRepository.Delete(resourceId);

            //TODO: add domain event
            _userActionService.Add(new AddUserActionDto()
            {
                UserId = resource.UserId,
                Date = DateTime.UtcNow,
                Action = UserActiontype.ResourceRemoved,
                Data = $@"Url: [:{resource.Url}], 
                        IsActivated: [old: {resource.MonitoringActivated}]"
            });
        }

        public async Task<Resource> Update(UpdateResourceDto resourcedto)
        {
            var updatedResource =  await _resourceRepository.Update(resourcedto);

            //TODO: add domain event
            _userActionService.Add(new AddUserActionDto()
            {
                UserId = updatedResource.UserId,
                Date = DateTime.UtcNow,
                Action = UserActiontype.ResourceUpdated,
                Data = $@"Url: [:{updatedResource.Url}], 
                        IsActivated: [old: {updatedResource.MonitoringActivated}]"
            });

            return updatedResource;
        }

        public Dictionary<Resource, MonitorItem> GetMonitoredItemsByPeriodicity(int periodicity)
        {
            return _resourceRepository.GetMonitoredItemsByPeriodicity(periodicity);
        }
    }
}