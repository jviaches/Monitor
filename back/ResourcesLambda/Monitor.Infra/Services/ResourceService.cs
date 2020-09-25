using Monitor.Core.Dto;
using Monitor.Core.Enums;
using Monitor.Infra.Entities;
using Monitor.Infra.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monitor.Infra.Services
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

            _userActionService.Add(new UserActionDto()
            {
                UserId = resourcedto.UserId,
                Date = DateTime.UtcNow,
                Action = UserActiontype.ResourceAdded,
                Data = $@"Url: [{resourcedto.Url}], 
                       IsActivated: [{resourcedto.IsMonitorActivated}], 
                       Activation Period: [{resourcedto.MonitorPeriod}]"
            });

            return _resourceRepository.Add(resource);
        }

        public async Task<IEnumerable<Resource>> GetByPeriodicityAndMonitor(int periodicity, bool isMonitored)
        {
            return await _resourceRepository.GetByPeriodicityAndMonitor(periodicity, isMonitored);
        }

        public async Task Update(UpdateResourceDto updateResourceDto)
        {
            var existingResource = await _resourceRepository.GetById(updateResourceDto.Id);
            
            var newResource = existingResource;
            newResource.IsMonitorActivated = updateResourceDto.IsMonitorActivated;
            newResource.MonitorActivationDate = DateTime.UtcNow;
            newResource.MonitorPeriod = updateResourceDto.MonitorPeriod;
            newResource.Url = updateResourceDto.Url;

            _resourceRepository.Update(newResource);

            _userActionService.Add(new UserActionDto()
            {
                UserId = existingResource.UserId,
                Date = DateTime.UtcNow,
                Action = UserActiontype.ResourceUpdated,
                Data = $@"Url: [old: {existingResource.Url}, new: {newResource.Url}], 
                        IsActivated: [old: {existingResource.IsMonitorActivated}, new: {newResource.IsMonitorActivated}], 
                        Activation Period: [old: {existingResource.MonitorPeriod}, new: {newResource.MonitorPeriod}]"
            });
        }

        public async Task Delete(int resourceId)
        {
            var resource = await _resourceRepository.GetById(resourceId);
            _resourceRepository.Delete(resource);

            _userActionService.Add(new UserActionDto()
            {
                UserId = resource.UserId,
                Date = DateTime.UtcNow,
                Action = UserActiontype.ResourceRemoved,
                Data = $@"Url: [:{resource.Url}], 
                        IsActivated: [old: {resource.IsMonitorActivated}], 
                        Activation Period: [old: {resource.MonitorPeriod}]"
            });
        }
    }
}
