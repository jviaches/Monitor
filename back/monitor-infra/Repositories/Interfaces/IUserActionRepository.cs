using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace monitor_infra.Repositories.Interfaces
{
    public interface IUserActionRepository
    {
        UserAction Add(UserAction entity);
        Task<UserAction> GetById(Guid id);
        Task<IEnumerable<UserAction>> GetByUserId(int id);
    }
}
