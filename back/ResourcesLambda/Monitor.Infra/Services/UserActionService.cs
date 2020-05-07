using Monitor.Core.Dto;
using Monitor.Core.Validations;
using Monitor.Infra.Entities;
using Monitor.Infra.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Infra.Services
{
    public class UserActionService : IUserActionService
    {
        private IUserActionRepository _userActionRepository;

        public UserActionService(IUserActionRepository userActionRepository)
        {
            _userActionRepository = userActionRepository;
        }

        public UserAction Add(UserActionDto userActionDto)
        {
            var userAction = new UserAction()
            {
                UserId = userActionDto.UserId,
                Date = DateTime.UtcNow,
                Action = userActionDto.Action,
                Data = userActionDto.Data
            };

            return _userActionRepository.Add(userAction);
        }

        public Task<UserAction> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserAction>> GetByPeriodicityAndMonitor(int periodicity, bool isMonitored)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserAction>> GetByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
