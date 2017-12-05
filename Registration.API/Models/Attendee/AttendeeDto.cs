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
        public string ShirtSize { get; set; }

        public ICollection<AccommodationDto> Accommodations { get; set; }

        public ICollection<MeritBadgeDto> MeritBadges { get; set; }
    }
}
