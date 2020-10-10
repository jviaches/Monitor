using monitor_core.Dto;
using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace monitor_infra.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User Add(AddUserDto addUserDto);
        User GetById(int userId);
        User GetByEmail(string email);
        Task<bool> Activate(string email, string activationCode);
        User Update(User user);
    }
}
