using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.API.Entities
{
    public class Attendee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public bool IsAdult { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool Triathlon { get; set; }
        
        [Required]
        public ShirtSize ShirtSize { get; set; }

        public ICollection<AttendeeAccommodation> AttendeeAccommodations { get; set; }
               = new List<AttendeeAccommodation>();

        public ICollection<AttendeeMeritBadge> AttendeeMeritBadges { get; set; }
               = new List<AttendeeMeritBadge>();
    }
}
