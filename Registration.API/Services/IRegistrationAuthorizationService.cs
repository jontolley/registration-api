using Registration.API.Entities;
using System.Security.Claims;

namespace Registration.API.Services
{
    public interface IRegistrationAuthorizationService
    {
        string GetCurrentUserIdentifier(ClaimsPrincipal user);
        bool IsAuthorized(string userIdentifier, int subgroupId);
        Role GetRole(string role);
        void AddRole(User user, Role role);
        void RemoveRole(User user, Role role);
    }
}
