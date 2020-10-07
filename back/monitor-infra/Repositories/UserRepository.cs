using monitor_core.Dto;
using monitor_infra.Entities;
using monitor_infra.Repositories.Interfaces;
using monitor_infra.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace monitor_infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IEmailSenderService _emailService;

        public UserRepository(AppDbContext dbContext, IEmailSenderService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public Task<bool> Activate(string email, string activationCode)
        {
            var user = _dbContext.Users.SingleOrDefault(rs => rs.Email == email && rs.ActivationCode == activationCode);
            if (user != null)
            {
                user.ActivationCode = string.Empty;
                _dbContext.Set<User>().Update(user);
                _dbContext.SaveChanges();

                return Task<bool>.Factory.StartNew(() => true);
            }

            return Task<bool>.Factory.StartNew(() => false);
        }

        public User Add(AddUserDto addUserDto)
        {
            if (GetByEmail(addUserDto.Email) != null)
                return null;

            User user = new User()
            {
                Email = addUserDto.Email,
                Password = addUserDto.Password,
                RegistrationDate = DateTime.UtcNow,
                ActivationCode = Guid.NewGuid().ToString("n").Substring(0, 8)
            };

            _dbContext.Set<User>().Add(user);
            _dbContext.SaveChanges();

            _emailService.SendConfirmationAccountEmail(user.Email, user.ActivationCode);

            return user;
        }

        public User GetByEmail(string email)
        {
            var user = _dbContext.Users.SingleOrDefault(rs => rs.Email == email);
            return user;
        }

        public User GetById(int userId)
        {
            var user = _dbContext.Users.SingleOrDefault(rs => rs.Id == userId);
            return user;
        }
    }
}
