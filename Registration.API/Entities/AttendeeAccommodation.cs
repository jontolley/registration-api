using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.API.Entities
{
    public class AttendeeAccommodation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("AttendeeId")]
        public Attendee Attendee { get; set; }
        public int AttendeeId { get; set; }
    
        [Required]
        [ForeignKey("AccommodationId")]
        public Accommodation Accommodation { get; set; }
        public int AccommodationId { get; set; }
    }

}
