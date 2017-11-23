using Registration.API.Models;
using System.Threading.Tasks;

namespace Registration.API.Services.Email
{
    public interface IEmailService
    {
        void SendMessage(EmailMessage email);
    }
}
