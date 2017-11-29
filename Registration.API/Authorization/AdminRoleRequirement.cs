using Microsoft.AspNetCore.Authorization;

namespace Registration.API
{
    internal class AdminRoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; private set; }

        public AdminRoleRequirement()
        {
            Role = "admin";
        }
    }
}