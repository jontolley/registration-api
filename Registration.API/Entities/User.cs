using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.API.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string SubscriberId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        internal static bool HasClaim(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        [MaxLength(100)]
        public string Nickname { get; set; }

        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(1024)]
        public string PictureUrl { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
               = new List<UserRole>();

        public ICollection<UserSubgroup> UserSubgroups { get; set; }
               = new List<UserSubgroup>();

        public ICollection<Attendee> InsertedAttendees { get; set; }
               = new List<Attendee>();

        public ICollection<Attendee> UpdatedAttendees { get; set; }
       = new List<Attendee>();
    }
}
