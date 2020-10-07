using monitor_core.Dto;
using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_infra.Services.Interfaces
{
    public interface IUserActionService
    {
        UserAction Add(AddUserActionDto userActionDto);
    }
}
