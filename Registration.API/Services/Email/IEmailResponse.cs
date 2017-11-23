using Registration.API.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Registration.API.Services.Email
{
    public interface IEmailResponse
    {
        HttpStatusCode StatusCode { get; set; }

        HttpContent Body { get; set; }

        HttpResponseHeaders Headers { get; set; }
    }
}
