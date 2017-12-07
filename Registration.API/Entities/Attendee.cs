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
        [ForeignKey("SubgroupId")]
        public Subgroup Subgroup { get; set; }
        public int SubgroupId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public bool IsAdult { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        public bool Triathlon { get; set; }
        
        [Required]
        public ShirtSize ShirtSize { get; set; }

        [Required]
        public DateTime InsertedOn { get; set; }
        [Required]
        public int InsertedById { get; set; }
        [Required]
        public User InsertedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedById { get; set; }
        public User UpdatedBy { get; set; }

        public ICollection<AttendeeAccommodation> AttendeeAccommodations { get; set; }
               = new List<AttendeeAccommodation>();

        public ICollection<AttendeeMeritBadge> AttendeeMeritBadges { get; set; }
               = new List<AttendeeMeritBadge>();
    }
}
