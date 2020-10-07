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
        public void SendConfirmationAccountEmail(string senderAddress, string tempCode)
        {
            using AmazonSimpleEmailServiceClient client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USWest2);
            var sendRequest = new SendEmailRequest
            {
                Source = "support@projscope.com",
                Destination = new Destination { ToAddresses = new List<string> { senderAddress } },
                Message = new Message
                {
                    Subject = new Content("Account confirmation - Projscope"),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = getConfirmationAccountTemplate(senderAddress, tempCode)
                        },
                        Text = new Content
                        {
                            Charset = "UTF-8",
                            Data = getConfirmationAccountBodyTemplate(senderAddress, tempCode)
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

        private string getConfirmationAccountTemplate(string email, string tempCode)
        {
            // The HTML body of the email.
            return @$"<html>
            <head></head>
            <body>
              <p>Your verification code is ${tempCode} Please activate it here: http://localhost:4200/user/confirmation/${email}/${tempCode}</p>
            </body>
            </html>";
        }

        private string getConfirmationAccountBodyTemplate(string email, string tempCode)
        {
            return $"Your verification code is ${tempCode} Please activate it here: http://localhost:4200/user/confirmation/${email}/${tempCode}";
        }
    }
}
