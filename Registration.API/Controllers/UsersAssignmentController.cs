using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Entities;
using Registration.API.Models;
using Registration.API.Services;
using System;
using System.Security.Claims;

namespace Registration.API.Controllers
{
    [Route("api/users")]
    public class UsersAssignmentController : Controller
    {
        private IRegistrationRepository _registrationRepository;
        private const string _userRoleName = "user";

        public UsersAssignmentController(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        [Authorize]
        [HttpGet("assignment")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                if (!User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier && c.Issuer == "https://tolleyfam.auth0.com/"))
                {
                    return NotFound();
                }

                var userIdentifier = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier && c.Issuer == "https://tolleyfam.auth0.com/").Value;

                var user = _registrationRepository.GetUser(userIdentifier, includeRoles:false, includeSubgroups:true);

                if (user == null)
                {
                    return NotFound();
                }

                var userWithSubgroupsResult = Mapper.Map<UserWithSubgroupsDto>(user);
                return Ok(userWithSubgroupsResult);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [Authorize]
        [HttpPost("assignment")]
        public IActionResult AssignUser([FromBody] UserSubgroupDto userSubgroupDto)
        {
            if (userSubgroupDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier && c.Issuer == "https://tolleyfam.auth0.com/"))
                {
                    return BadRequest();
                }

                var userIdentifier = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier && c.Issuer == "https://tolleyfam.auth0.com/").Value;

                var user = _registrationRepository.GetUser(userIdentifier, includeRoles: false, includeSubgroups: true);

                if (user == null)
                {
                    return NotFound();
                }

                if (user.SubscriberId != userSubgroupDto.SubscriberId)
                {
                    return BadRequest();
                }

                _registrationRepository.RemoveAllAssignments(user);

                var hasAssignment = false;
                foreach (var assignment in userSubgroupDto.Subgroups)
                {
                    if (!_registrationRepository.CheckSubgroupPin(assignment.Id, assignment.Pin)) continue;

                    var userSubgroup = new UserSubgroup
                    {
                        UserId = user.Id,
                        SubgroupId = assignment.Id
                    };

                    _registrationRepository.AddAssignment(userSubgroup);
                    hasAssignment = true;
                }

                var role = _registrationRepository.GetRole(_userRoleName);
                if (hasAssignment)
                {
                    _registrationRepository.AddRole(user, role);
                }
                else
                {
                    _registrationRepository.RemoveRole(user, role);
                }

                if (!_registrationRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                var savedUser = _registrationRepository.GetUser(user.Id, includeRoles: false, includeSubgroups: true);

                var userToReturn = Mapper.Map<UserWithSubgroupsDto>(savedUser);

                return Created("api/user/current", userToReturn);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }
    }
}
