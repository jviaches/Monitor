using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using monitor_infra.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace monitor_infra.Services
{
    public class EmailSenderService : IEmailSenderService
    {

        private void sendEmail(string senderAddress, string subject, string htmlTemplate, string textTemplate)
        {
            using var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1);
            var sendRequest = new SendEmailRequest
            {
                Source = "support@projscope.com",
                Destination = new Destination { ToAddresses = new List<string> { senderAddress } },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = htmlTemplate
                        },
                        Text = new Content
                        {
                            Charset = "UTF-8",
                            Data = textTemplate
                        }
                    }
                },
            };
            try
            {
                var response = client.SendEmailAsync(sendRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("The email was not sent.");
                Console.WriteLine("Error message: " + ex.Message);
            }
        }

        public void SendConfirmationAccountEmail(string senderAddress, string tempCode)
        {
            var htmlTemplate = getConfirmationAccountHtmlTemplate(senderAddress, tempCode);
            var textTemplate = getConfirmationAccountTextTemplate(senderAddress, tempCode);

            sendEmail(senderAddress, "Account confirmation - Projscope", htmlTemplate, textTemplate);
        }

        private string getConfirmationAccountHtmlTemplate(string email, string tempCode)
        {
            // TODO: instead of localhost, send on server based on environment variable
            // The HTML body of the email.
            return @$"<html>
            <head></head>
            <body>
              <p>Your verification code is <b>{tempCode}</b>. Please activate it here: http://localhost:4200/user-confirmation/1?email={email}&code={tempCode} </p>
            </body>
            </html>";
        }

        private string getConfirmationAccountTextTemplate(string email, string tempCode)
        {
            // TODO: instead of localhost, send on server based on environment variable
            return $"Your verification code is {tempCode}. Please activate it here:  http://localhost:4200/user-confirmation/1?email={email}&code={tempCode}";
        }

        public void SendForgottenPassword(string email, string password)
        {
            var htmlTemplate = getEmailHtmlTemplate(password);
            var textTemplate = getEmailTextTemplate(password);

            sendEmail(email, "Password retrival - Projscope", htmlTemplate, textTemplate);
        }

        private string getEmailTextTemplate(string password)
        {
            return $"Your password is <b>{password}</b>. You can login here: http://projscope.com";
        }

        private string getEmailHtmlTemplate(string password)
        {
            return @$"<html>
            <head></head>
            <body>
              <p>Your password is <b>{password}</b>. You can login here: http://projscope.com </p>
            </body>
            </html>";
        }
    }
}
