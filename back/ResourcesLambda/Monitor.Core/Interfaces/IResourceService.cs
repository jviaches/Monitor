using Monitor.Core.Models;
using Monitor.Core.Validations;
using Monitor.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Core.Interfaces
{
    public interface IResourceService
    {
        Task<Resource> GetById(int id);
        Task<IEnumerable<Resource>> GetByUserId(int userId);
        Resource Add(ResourceViewModel resourceVM);
        //Task<Result> Update(UpdateResourceViewModel resourceVM);
        //Task<Result> Delete(UpdateResourceViewModel resourceVM);
        //Task<IEnumerable<ResourcesHistory>> GetHistoryByResourceId(string resourceId);
    }
}
