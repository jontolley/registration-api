using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.API.Entities
{
    public class UserRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }
    
        [Required]
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        public int RoleId { get; set; }
    }

}
