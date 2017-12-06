using Microsoft.EntityFrameworkCore;
using Registration.API.Entities;
using System.Linq;
using System.Security.Claims;

namespace Registration.API.Services
{
    public class RegistrationAuthorizationService : IRegistrationAuthorizationService
    {
        private const string _claimsIssuer = "https://tolleyfam.auth0.com/";
        private RegistrationContext _context;

        public RegistrationAuthorizationService(RegistrationContext context)
        {
            _context = context;
        }

        public string GetCurrentUserIdentifier(ClaimsPrincipal user)
        {
            if (!user.HasClaim(c => c.Type == ClaimTypes.NameIdentifier && c.Issuer == _claimsIssuer))
            {
                return null;
            }

            return user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier && c.Issuer == _claimsIssuer).Value;
        }

        public bool IsAuthorized(string subscriberId, int subgroupId)
        {
            var user = _context.Users.Include(u => u.UserSubgroups).FirstOrDefault(u => u.SubscriberId == subscriberId);
            return user.UserSubgroups.Any(us => us.SubgroupId == subgroupId);
        }
    }
}
