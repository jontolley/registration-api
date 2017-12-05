using System.ComponentModel.DataAnnotations;

namespace Registration.API.Models
{
    public class SubgroupWithPinDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Pin { get; set; }
    }
}
