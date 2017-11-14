namespace Registration.API.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        public string SubscriberId { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public string Email { get; set; }

        public string PictureUrl { get; set; }
    }
}
