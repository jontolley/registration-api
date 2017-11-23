using System;
using System.Collections.Generic;

namespace Registration.API.Models
{
    public class EmailMessage
    {
        public string From { get; set; }

        public string Subject { get; set; }

        public string ContentType { get; set; }

        public string Content { get; set; }

        public IEnumerable<string> To { get; set; }
    }
}
