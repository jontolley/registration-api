using Microsoft.AspNetCore.Hosting;
using Registration.API.Models;
using Registration.API.Models.Contacts;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Registration.API.Services.Email
{
    public class SendGridService : IEmailService
    {
        // https://docs.microsoft.com/en-us/azure/sendgrid-dotnet-how-to-send-email

        private readonly string _apiKey;
        private readonly string _contactEmail;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SendGridService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            _apiKey = Startup.Configuration["appSettings:sendGridApiKey"];
            _contactEmail = Startup.Configuration["appSettings:contactEmail"];
        }

        public async Task<IEmailResponse> SendMessage(EmailMessage email)
        {
            var client = new SendGridClient(_apiKey);
            var message = new SendGridMessage()
            {
                From = new EmailAddress(email.From),
                Subject = email.Subject
            };

            if(email.ContentType == EmailContentType.Html)
            {
                message.HtmlContent = email.Message;
            } else
            {
                message.PlainTextContent = email.Message;
            }

            foreach (var recipient in email.To)
            {
                message.AddTo(recipient);
            }            

            var sendGridResponse = await client.SendEmailAsync(message);

            var response = new SimpleEmailResponse()
            {
                Headers = sendGridResponse.Headers,
                StatusCode = sendGridResponse.StatusCode,
                Body = sendGridResponse.Body
            };

            return response;
        }

        public async Task<EmailMessage> GenerateEmailMessageAsync(ContactDto contactDto)
        {
            var email = new EmailMessage
            {
                To = new[] { _contactEmail },
                From = "contact@sunrise2018.org",
                Subject = "LDS Encampment Message",
                ContentType = EmailContentType.Html,
                Message = await BuildEmailMessageAsync(contactDto)
            };

            return email;
        }

        private async Task<string> BuildEmailMessageAsync(ContactDto contactInfo)
        {
            string template;

            var assembly = Assembly.GetEntryAssembly();
            var resourceStream = assembly.GetManifestResourceStream("Registration.API.Services.Email.Templates.contactUs.html");
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                template = await reader.ReadToEndAsync();
            }

            var message = template
                .Replace("__FIRST_NAME", contactInfo.FirstName)
                .Replace("__LAST_NAME", contactInfo.LastName)
                .Replace("__EMAIL", contactInfo.Email)
                .Replace("__MESSAGE", contactInfo.Message)
                .Replace("__PHONE_NUMBER", contactInfo.PhoneNumber)
                .Replace("__STAKE", contactInfo.Stake)
                .Replace("__WARD", contactInfo.Ward);

            return message;
        }
    }
}
