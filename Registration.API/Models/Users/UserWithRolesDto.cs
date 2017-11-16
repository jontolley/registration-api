using System.Collections.Generic;

namespace Registration.API.Models
{
    public class UserWithRolesDto
    {
        public string SubscriberId { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public string Email { get; set; }

        public string PictureUrl { get; set; }

        public int NumberOfRoles
        {
            get
            {
                return Roles.Count;
            }
        }
        public ICollection<RoleDto> Roles { get; set; }
    }
}
