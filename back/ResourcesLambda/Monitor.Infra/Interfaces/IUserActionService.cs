using Monitor.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monitor.Infra.Entities
{
    public interface IUserActionService
    {
        Task<UserAction> GetById(int id);
        Task<IEnumerable<UserAction>> GetByUserId(int userId);
        Task<IEnumerable<UserAction>> GetByPeriodicityAndMonitor(int periodicity, bool isMonitored);
        UserAction Add(UserActionDto userActionDto);
    }
}
