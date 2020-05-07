using Monitor.Core.Dto;
using Monitor.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infra.Entities
{
    public interface IResourceHistoryService
    {
        Task<ResourcesHistory> GetById(int id);
        Task<ResourcesHistory> ResourceId(int resourceId);
        ResourcesHistory Add(AddResourceHistoryDto resourcedto);
        //Task<Result> Update(UpdateResourceViewModel resourceVM);
        //Task<Result> Delete(UpdateResourceViewModel resourceVM);
        //Task<IEnumerable<ResourcesHistory>> GetHistoryByResourceId(string resourceId);
    }
}
