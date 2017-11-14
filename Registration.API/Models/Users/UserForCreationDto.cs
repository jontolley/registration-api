using System.ComponentModel.DataAnnotations;

namespace Registration.API.Models
{
    public class UserForCreationDto
    {
        [Required(ErrorMessage = "You must provide a SubscriberID value.")]
        [MaxLength(50)]
        public string SubscriberId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Nickname { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(1024)]
        public string PictureUrl { get; set; }
    }
}
