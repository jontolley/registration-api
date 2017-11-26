using System.Net;
using System.Net.Http;

namespace Registration.API.Services.Email
{
    public interface IEmailResponse
    {
        HttpStatusCode StatusCode { get; set; }

        HttpContent Body { get; set; }
    }
}
