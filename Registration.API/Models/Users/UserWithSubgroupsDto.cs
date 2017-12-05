using System.Collections.Generic;

namespace Registration.API.Models
{
    public class UserWithSubgroupsDto
    {
        public string SubscriberId { get; set; }

        public string Name { get; set; }

        public int NumberOfSubgroups
        {
            get
            {
                return Subgroups.Count;
            }
        }
        public ICollection<SubgroupDto> Subgroups { get; set; }
    }
}
