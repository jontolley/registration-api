using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Models;
using Registration.API.Services;
using System;
using System.Security.Claims;

namespace Registration.API.Controllers
{
    [Route("api/users")]
    public class UsersCurrentController : Controller
    {
        private IRegistrationRepository _registrationRepository;

        public UsersCurrentController(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        [Authorize]
        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                if (!User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier && c.Issuer == "https://tolleyfam.auth0.com/"))
                {
                    return NotFound();
                }

                var userIdentifier = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier && c.Issuer == "https://tolleyfam.auth0.com/").Value;

                var user = _registrationRepository.GetUser(userIdentifier, includeRoles:true);

                if (user == null)
                {
                    return NotFound();
                }

                var userWithRolesResult = Mapper.Map<UserWithRolesDto>(user);
                return Ok(userWithRolesResult);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }
    }
}
