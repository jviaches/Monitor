using Monitor.Core.Dto;
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
