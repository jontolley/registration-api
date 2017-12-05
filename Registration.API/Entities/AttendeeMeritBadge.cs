using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.API.Entities
{
    public class AttendeeMeritBadge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int SortOrder { get; set; }

        [Required]
        [ForeignKey("AttendeeId")]
        public Attendee Attendee { get; set; }
        public int AttendeeId { get; set; }
    
        [Required]
        [ForeignKey("MeritBadgeId")]
        public MeritBadge MeritBadge { get; set; }
        public int MeritBadgeId { get; set; }
    }
}
