using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace monitor_infra.Services.Interfaces
{
    public interface IEmailSenderService
    {
        void SendConfirmationAccountEmail(string senderAddress, string tempCode);
        void SendForgottenPassword(string email, string password);
    }
}
