﻿namespace Registration.API.Models
{
    public class AttendanceDto
    {
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }

        public int DaysAttending { get; set; }
    }
}
