using Registration.API.Models;
using System.Threading.Tasks;

namespace Registration.API.Services.Email
{
    public interface IEmailService
    {
        Task<IEmailResponse> SendMessage(EmailMessage email);
        Task<EmailMessage> GenerateEmailMessageAsync(ContactDto contactDto);
    }
}
