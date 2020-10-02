using Monitor.Core.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infra.Entities
{
    public interface IResourceService
    {
        Task<Resource> GetById(int id);
        Task<IEnumerable<Resource>> GetByUserId(Guid userId);
        Task<IEnumerable<Resource>> GetByPeriodicityAndMonitor(int periodicity, bool isMonitored);
        Resource Add(AddResourceDto resourcedto);
        Task Update(UpdateResourceDto resource);
        Task Delete(int resourceId);
        //Task<IEnumerable<ResourcesHistory>> GetHistoryByResourceId(string resourceId);
    }
}
