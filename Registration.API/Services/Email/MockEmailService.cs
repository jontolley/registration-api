using Registration.API.Models;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Registration.API.Services.Email
{
    public class MockEmailService : IEmailService
    {
        private readonly IEnumerable<string> _contactEmails;

        public MockEmailService()
        {
            var contactEmails = Startup.Configuration["appSettings:contactEmail"];
            _contactEmails = contactEmails.Split(',');
        }

        public async Task<EmailMessage> GenerateEmailMessageAsync(ContactDto contactDto)
        {
            var email = new EmailMessage
            {
                To = _contactEmails,
                From = "contact@sunrise2018.org",
                Subject = "LDS Encampment Message",
                ContentType = EmailContentType.Html,
                Message = await BuildEmailMessageAsync(contactDto)
            };

            return email;
        }

        public async Task<IEmailResponse> SendMessage(EmailMessage email)
        {
            await Task.Delay(1000);

            var response = new SimpleEmailResponse()
            {
                StatusCode = System.Net.HttpStatusCode.Accepted,
                Body = null
            };

            return response;
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
                .Replace("__NAME", contactInfo.Name)
                .Replace("__EMAIL", contactInfo.Email)
                .Replace("__MESSAGE", contactInfo.Message)
                .Replace("__PHONE_NUMBER", contactInfo.PhoneNumber)
                .Replace("__STAKE", contactInfo.Stake)
                .Replace("__WARD", contactInfo.Ward);

            return message;
        }
    }
}
