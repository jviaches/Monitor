using monitor_core.Dto;
using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace monitor_infra.Services.Interfaces
{

    public interface IResourceService
    {
        Task<Resource> GetById(Guid id);
        Task<IEnumerable<Resource>> GetByUserId(int userId);
        Resource Add(AddResourceDto resourcedto);
        Task<Resource> Update(UpdateResourceDto resourcedto);
        Task Delete(Guid resourceId);
        Dictionary<Resource, MonitorItem> GetMonitoredItemsByPeriodicity(int periodicity);
    }
}
