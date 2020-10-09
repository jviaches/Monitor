using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace monitor_infra.Services.Interfaces
{
    public interface IUserService
    {
        User Add(string email, string password);
        User SignIn(string email, string password);
        User GetByEmail(string email);
        User GetById(int id);
        Task<bool> Activate(string email, string activationCode);
        void ResendActivationCode(string email);
        bool ChangePassword(string email, string oldPassword, string newPassword);
        void ResendPassword(string email);
    }
}
