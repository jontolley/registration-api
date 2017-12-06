﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Registration.API.Entities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Registration.API
{
    internal class AdminRoleHandler : AuthorizationHandler<AdminRoleRequirement>
    {
        private const string _claimsIssuer = "https://tolleyfam.auth0.com/";

        public AdminRoleHandler()
        { }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRoleRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier && c.Issuer == _claimsIssuer))
            {
                return Task.CompletedTask;
            }
            
            var userIdentifier = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier && c.Issuer == _claimsIssuer).Value;

            var contextFactory = new RegistrationDesignTimeContextFactory();

            using (var db = contextFactory.CreateDbContext(new string[] { }))
            {
                var user = db.Users.Include(g => g.UserRoles).ThenInclude(ur => ur.Role).Where(g => g.SubscriberId == userIdentifier).FirstOrDefault();

                if (user != null)
                {
                    foreach (var role in user.UserRoles)
                    {
                        if (role.Role.Name == requirement.Role) context.Succeed(requirement);
                    }
                }
                    

                return Task.CompletedTask;
            }
        }
    }
}