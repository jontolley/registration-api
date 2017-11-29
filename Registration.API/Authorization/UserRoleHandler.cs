using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Registration.API.Entities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Registration.API
{
    internal class UserRoleHandler : AuthorizationHandler<UserRoleRequirement>
    {
        public UserRoleHandler()
        { }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRoleRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier && c.Issuer == "https://tolleyfam.auth0.com/"))
            {
                return Task.CompletedTask;
            }
            
            var userIdentifier = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier && c.Issuer == "https://tolleyfam.auth0.com/").Value;

            var contextFactory = new RegistrationDesignTimeContextFactory();

            using (var db = contextFactory.CreateDbContext(new string[] { }))
            {
                var user = db.Users.Include(g => g.UserRoles).ThenInclude(ur => ur.Role).Where(g => g.SubscriberId == userIdentifier).FirstOrDefault();

                foreach (var role in user.UserRoles)
                {
                    if (role.Role.Name == requirement.Role) context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}