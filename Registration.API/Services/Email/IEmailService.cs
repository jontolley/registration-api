using Registration.API.Models;
using System.Threading.Tasks;
using Registration.API.Models.Contacts;

namespace Registration.API.Services.Email
{
    public interface IEmailService
    {
        Task<IEmailResponse> SendMessage(EmailMessage email);
        Task<EmailMessage> GenerateEmailMessageAsync(ContactDto contactDto);
    }
}
