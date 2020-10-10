using monitor_core.Dto;
using monitor_infra.Entities;
using monitor_infra.Repositories.Interfaces;
using monitor_infra.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace monitor_infra.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IEncryptionService _encryptionService;
        private readonly IEmailSenderService _emailService;

        public UserService(IUserRepository userRepository, IEmailSenderService emailService, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _emailService = emailService;
        }

        public Task<bool> Activate(string email, string activationCode)
        {
            return _userRepository.Activate(email, activationCode);
        }

        public User Add(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new Exception("Email or password is empty!");
            
            var user = _userRepository.Add(new AddUserDto()
            {
                Email = email,
                Password = password
            });

            _emailService.SendConfirmationAccount(user.Email, user.ActivationCode);

            return user;
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

        public void ResendActivationCode(string email)
        {
            var user = _userRepository.GetByEmail(email);
            if (user == null && string.IsNullOrEmpty(user.ActivationCode))
                return;

            user.GenerateActivationCode();
            _userRepository.Update(user);
            _emailService.SendConfirmationAccount(email, user.ActivationCode);
        }

        public User SignIn(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new Exception("Empty email or password !");

            var user = _userRepository.GetByEmail(email);

            if (!string.IsNullOrEmpty(user.ActivationCode))
                return null;

            var decryptedPassword = _encryptionService.Decrypt(user.Password);

            if (user != null && password == decryptedPassword)
                return user;

            return null;
        }

        public bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
                return false;

            if (oldPassword == newPassword)
                return false;

            var user = _userRepository.GetByEmail(email);

            var decryptedPassword = _encryptionService.Decrypt(user.Password);
            if (oldPassword != decryptedPassword)
                return false;

            var pass = _encryptionService.Encrypt(newPassword);
            user.Password = pass;

            _userRepository.Update(user);

            return true;
        }

        public void ResendPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return;

            var user = _userRepository.GetByEmail(email);
            var decryptedPassword = _encryptionService.Decrypt(user.Password);

            _emailService.SendForgottenPassword(user.Email, decryptedPassword);
        }
    }
}
