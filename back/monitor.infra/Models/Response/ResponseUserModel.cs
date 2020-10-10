using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_infra.Models.Response
{
    public class ResponseUserModel
    {
        public int Id { get; private set; }
        public string Email { get; private set; }
        public DateTime RegistrationDate { get; private set; }
        public string Token { get; private set; }

        public ResponseUserModel(User user, string token)
        {
            Id = user.Id;
            Email = user.Email;
            RegistrationDate = user.RegistrationDate;
            Token = token;
        }
    }
}
