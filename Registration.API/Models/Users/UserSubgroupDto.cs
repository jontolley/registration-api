using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Registration.API.Models
{
    public class UserSubgroupDto
    {
        [Required]
        public string SubscriberId { get; set; }

        public ICollection<SubgroupWithPinDto> Subgroups { get; set; }
    }
}
