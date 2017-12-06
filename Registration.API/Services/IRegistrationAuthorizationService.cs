using System.Security.Claims;

namespace Registration.API.Services
{
    public interface IRegistrationAuthorizationService
    {
        string GetCurrentUserIdentifier(ClaimsPrincipal user);
        bool IsAuthorized(string userIdentifier, int subgroupId);
    }
}
