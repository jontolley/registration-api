using AutoMapper;
using Registration.API.Entities;
using Registration.API.Models;
using System.Collections.Generic;

namespace Registration.API.CustomDtoMapper
{
    public class UserToUserWithRolesDtoConverter : ITypeConverter<User, UserWithRolesDto>
    {
        public UserWithRolesDto Convert(User user, UserWithRolesDto userWithRolesDto, ResolutionContext context)
        {
            userWithRolesDto = new UserWithRolesDto();

            var roles = new List<string>();

            foreach (var userRole in user.UserRoles)
            {
                roles.Add(userRole.Role.Name.ToLower());
            }

            userWithRolesDto.SubscriberId = user.SubscriberId;
            userWithRolesDto.Name = user.Name;
            userWithRolesDto.Nickname = user.Nickname;
            userWithRolesDto.Email = user.Email;
            userWithRolesDto.PictureUrl = user.PictureUrl;
            userWithRolesDto.Roles = roles;

            return userWithRolesDto;
        }
    }
}
