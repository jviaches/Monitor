using monitor_core.Dto;
using monitor_infra.Entities;
using monitor_infra.Repositories.Interfaces;
using monitor_infra.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_infra.Services
{
    public class UserActionService : IUserActionService
    {
        private IUserActionRepository _userActionRepository;

        public UserActionService(IUserActionRepository userActionRepository)
        {
            _userActionRepository = userActionRepository;
        }

        public UserAction Add(AddUserActionDto userActionDto)
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
    }
}
