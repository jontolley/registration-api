using System.ComponentModel.DataAnnotations;

namespace Registration.API.Models
{
    public class ContactForCreationDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Message { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        public string Ward { get; set; }

        [MaxLength(100)]
        public string Stake { get; set; }
    }
}
