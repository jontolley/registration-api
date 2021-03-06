﻿using System;

namespace Registration.API.Models
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string PhoneNumber { get; set; }
        public string Ward { get; set; }
        public string Stake { get; set; }
        public DateTime ReceivedDateTime { get; set; }
    }
}
