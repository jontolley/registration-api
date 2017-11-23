using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Registration.API.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Registration.API.Services.Email
{
    public class SendGridService : IEmailService
    {
        // https://docs.microsoft.com/en-us/azure/sendgrid-dotnet-how-to-send-email

        private readonly string API_KEY;

        public SendGridService()
        {
            API_KEY = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
        }
        
        public void SendMessage(EmailMessage email)
        {
            var client = new SendGridClient(API_KEY);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("test@example.com", "DX Team"),
                Subject = "Hello World from the SendGrid CSharp SDK!",
                PlainTextContent = "Hello, Email!",
                HtmlContent = "<strong>Hello, Email!</strong>"
            };

            msg.AddTo(new EmailAddress("test@example.com", "Test User"));
            //var response = client.SendEmailAsync(msg).Wait()
        }
    }
}
