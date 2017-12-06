using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Registration.API.Models
{
    public class AttendeeForCreationDto
    {
        [Required(ErrorMessage = "You must provide a SubgroupId.")]
        public int SubgroupId { get; set; }

        [Required(ErrorMessage = "You must provide a First Name value.")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must provide a Last Name value.")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You must provide a value for IsAdult.")]
        public bool IsAdult { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public bool Triathlon { get; set; }

        [Required(ErrorMessage = "You must provide a Shrit Size.")]
        public string ShirtSize { get; set; }

        public ICollection<AccommodationDto> Accommodations { get; set; }

        public ICollection<MeritBadgeForCreationDto> MeritBadges { get; set; }
    }
}
