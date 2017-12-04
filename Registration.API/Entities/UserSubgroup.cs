using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.API.Entities
{
    public class UserSubgroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }
    
        [Required]
        [ForeignKey("SubgroupId")]
        public Subgroup Subgroup { get; set; }
        public int SubgroupId { get; set; }
    }

}
