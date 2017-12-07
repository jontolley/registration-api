using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Entities;
using Registration.API.Models;
using Registration.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public IActionResult GetAssignment()
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
        public IActionResult PostAssignment([FromBody] IEnumerable<SubgroupWithPinDto> subgroupWithPinDtos)
        {
            if (subgroupWithPinDtos == null)
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

                _registrationRepository.RemoveAllAssignments(user);

                var hasAssignment = false;
                foreach (var assignment in subgroupWithPinDtos)
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

                var role = _registrationAuthorizationService.GetRole(_userRoleName);
                if (hasAssignment)
                {
                    _registrationAuthorizationService.AddRole(user, role);
                }
                else
                {
                    _registrationAuthorizationService.RemoveRole(user, role);
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

        [Authorize]
        [HttpPut]
        public IActionResult PutAssignment([FromBody] SubgroupWithPinDto subgroupWithPinDto)
        {
            if (subgroupWithPinDto == null)
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

                if (_registrationRepository.CheckSubgroupPin(subgroupWithPinDto.Id, subgroupWithPinDto.Pin))
                {
                    // Check if assignment already exists, if it doesn't then add it.
                    if (!user.UserSubgroups.Any(us => us.SubgroupId == subgroupWithPinDto.Id))
                    {
                        _registrationRepository.AddAssignment(new UserSubgroup
                        {
                            UserId = user.Id,
                            SubgroupId = subgroupWithPinDto.Id
                        });
                    }                    
                }
                else
                {
                    return BadRequest();
                }

                var role = _registrationAuthorizationService.GetRole(_userRoleName);
                _registrationAuthorizationService.AddRole(user, role);

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

        [Authorize]
        [HttpDelete("{subgroupId}")]
        public IActionResult DeleteAssignment(int subgroupId)
        {
            var userIdentifier = _registrationAuthorizationService.GetCurrentUserIdentifier(User);
            if (userIdentifier == null)
            {
                return BadRequest();
            }

            if (!_registrationAuthorizationService.IsAuthorized(userIdentifier, subgroupId))
            {
                return NotFound();
            }

            var user = _registrationRepository.GetUser(userIdentifier, includeRoles: false, includeSubgroups: true);
            if (user == null)
            {
                return NotFound();
            }

            _registrationRepository.RemoveAssignment(user, subgroupId);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
