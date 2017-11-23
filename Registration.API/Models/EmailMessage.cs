using System;
using System.Collections.Generic;

namespace Registration.API.Models
{
    public enum EmailContentType
    {
        Text, Html
    }

    public class EmailMessage
    {
        public string From { get; set; }

        public string Subject { get; set; }

        public EmailContentType ContentType { get; set; }

        public string Message { get; set; }

        public IEnumerable<string> To { get; set; }
    }
}
