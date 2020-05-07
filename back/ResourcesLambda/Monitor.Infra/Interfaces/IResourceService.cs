using Monitor.Core.Dto;
using Monitor.Infra.Entities;
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
        //Task<Result> Update(UpdateResourceViewModel resourceVM);
        //Task<Result> Delete(UpdateResourceViewModel resourceVM);
        //Task<IEnumerable<ResourcesHistory>> GetHistoryByResourceId(string resourceId);
    }
}
