using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Registration.API.Services.Email
{
    public class SimpleEmailResponse : IEmailResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public HttpContent Body { get; set; }
        public HttpResponseHeaders Headers { get; set; }
    }
}
