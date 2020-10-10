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
        private readonly IEncryptionService _encryptionService;

        public UserRepository(AppDbContext dbContext, IEncryptionService encryptionService)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
        }

        public Task<bool> Activate(string email, string activationCode)
        {
            var user = _dbContext.Users.SingleOrDefault(rs => rs.Email == email && rs.ActivationCode == activationCode);
            if (user != null)
            {
                user.RemoveActivationCode();
                _dbContext.Update(user);
                _dbContext.SaveChanges();

                return Task<bool>.Factory.StartNew(() => true);
            }

            return Task<bool>.Factory.StartNew(() => false);
        }

        public User Update(User existingUser)
        {
            var user = GetByEmail(existingUser.Email);
            if (user != null)
            {
                _dbContext.Update(user);
                _dbContext.SaveChanges();
            }

            return user;
        }

        public User Add(AddUserDto addUserDto)
        {
            if (GetByEmail(addUserDto.Email) != null)
                return null;

            User user = new User()
            {
                Email = addUserDto.Email,
                Password = _encryptionService.Encrypt(addUserDto.Password),
                RegistrationDate = DateTime.UtcNow
            };

            user.GenerateActivationCode();

            _dbContext.Set<User>().Add(user);
            _dbContext.SaveChanges();

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
