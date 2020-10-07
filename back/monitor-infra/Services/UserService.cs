using monitor_core.Dto;
using monitor_infra.Entities;
using monitor_infra.Repositories.Interfaces;
using monitor_infra.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace monitor_infra.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<bool> Activate(string email, string activationCode)
        {
            return _userRepository.Activate(email, activationCode);
        }

        public User Add(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new Exception("Email or password is empty!");
            
            return _userRepository.Add(new AddUserDto()
            {
                Email = email,
                Password = password
            });
        }

        public User GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new Exception("Email is empty!");

            return _userRepository.GetByEmail(email);
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }
    }
}
