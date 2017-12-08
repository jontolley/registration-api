using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.API.Entities
{
    public class ShirtSize
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Size { get; set; }

        [Required]
        public int SortOrder { get; set; }

        public ICollection<Attendee> ShirtSizeAttendees { get; set; }
               = new List<Attendee>();
    }
}
