using AutoMapper;
using Registration.API.Entities;
using Registration.API.Models;
using System.Collections.Generic;

namespace Registration.API.CustomDtoMapper
{
    public class UserToUserWithSubgroupsDtoConverter : ITypeConverter<User, UserWithSubgroupsDto>
    {
        public UserWithSubgroupsDto Convert(User user, UserWithSubgroupsDto userWithSubgroupsDto, ResolutionContext context)
        {
            userWithSubgroupsDto = new UserWithSubgroupsDto();

            var subgroups = new List<Subgroup>();

            foreach (var userSubgroup in user.UserSubgroups)
            {
                subgroups.Add(userSubgroup.Subgroup);
            }

            userWithSubgroupsDto.SubscriberId = user.SubscriberId;
            userWithSubgroupsDto.Name = user.Name;

            var subgroupsDto = Mapper.Map<ICollection<SubgroupDto>>(subgroups);

            userWithSubgroupsDto.Subgroups = subgroupsDto;

            return userWithSubgroupsDto;
        }
    }
}
