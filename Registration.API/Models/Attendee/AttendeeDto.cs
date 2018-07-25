using System;
using System.Collections.Generic;

namespace Registration.API.Models
{
    public class AttendeeDto
    {
        public int Id { get; set; }
        public int SubgroupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdult { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool Triathlon { get; set; }
        public bool IsWithMinor { get; set; }
        public string ShirtSize { get; set; }

        public string InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public ICollection<AccommodationDto> Accommodations { get; set; }

        public ICollection<MeritBadgeDto> MeritBadges { get; set; }

        public AttendanceDto Attendance { get; set; }
    }
}
