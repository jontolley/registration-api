using System.ComponentModel.DataAnnotations;

namespace Registration.API.Models
{
    public class SubgroupForUpdateDto
    {
        [Required(ErrorMessage = "You must provide a name value.")]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}
