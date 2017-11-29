using Microsoft.AspNetCore.Authorization;

namespace Registration.API
{
    internal class UserRoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; private set; }

        public UserRoleRequirement()
        {
            Role = "user";
        }
    }
}