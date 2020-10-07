using monitor_core.Dto;
using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace monitor_infra.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        Resource Add(AddResourceDto addResourceDto);
        Task<Resource> Delete(Guid resourceId);
        Task<Resource> GetById(Guid id);
        Task<IEnumerable<Resource>> GetByUserId(int userId);
        Task<Resource> Update(UpdateResourceDto resourcedto);
    }
}
