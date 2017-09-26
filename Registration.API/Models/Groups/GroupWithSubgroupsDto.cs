using System.Collections.Generic;

namespace Registration.API.Models
{
    public class GroupWithSubgroupsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
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
