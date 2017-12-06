using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Entities;
using Registration.API.Models;
using Registration.API.Services;
using System;

namespace Registration.API.Controllers
{
    [Route("api/users/current/assignment")]
    public class UsersAssignmentController : Controller
    {
        private IRegistrationRepository _registrationRepository;
        private IRegistrationAuthorizationService _registrationAuthorizationService;

        private const string _userRoleName = "user";

        public UsersAssignmentController(IRegistrationRepository registrationRepository, IRegistrationAuthorizationService registrationAuthorizationService)
        {
            _registrationRepository = registrationRepository;
            _registrationAuthorizationService = registrationAuthorizationService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var userIdentifier = _registrationAuthorizationService.GetCurrentUserIdentifier(User);
                if (userIdentifier == null)
                {
                    return BadRequest();
                }

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
        [HttpPost]
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
                var userIdentifier = _registrationAuthorizationService.GetCurrentUserIdentifier(User);
                if (userIdentifier == null)
                {
                    return BadRequest();
                }

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
