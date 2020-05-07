using Monitor.Infra.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monitor.Infra.Interfaces.Repository
{
    public interface IResourceRepository : IRepository<Resource>
    {
        Task<IEnumerable<Resource>> GetByUserId(int id);
        Task<IEnumerable<Resource>> GetByPeriodicityAndMonitor(int periodicity, bool isMonitored);
    }
}
