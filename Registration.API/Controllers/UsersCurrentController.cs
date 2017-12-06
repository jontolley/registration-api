using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Registration.API.Entities;
using Registration.API.Models;
using Registration.API.Services;
using System;
using System.Data.SqlClient;

namespace Registration.API.Controllers
{
    [Route("api/users/current")]
    public class UsersCurrentController : Controller
    {
        private IRegistrationRepository _registrationRepository;
        private IRegistrationAuthorizationService _registrationAuthorizationService;

        public UsersCurrentController(IRegistrationRepository registrationRepository, IRegistrationAuthorizationService registrationAuthorizationService)
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

        [Authorize]
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserForCreationDto userForCreationDto)
        {
            if (userForCreationDto == null)
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
                if (userIdentifier == null || userIdentifier != userForCreationDto.SubscriberId)
                {
                    return BadRequest();
                }

                var userEntity = Mapper.Map<User>(userForCreationDto);

                _registrationRepository.AddUser(userEntity);

                if (!_registrationRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                var createdUserToReturn = Mapper.Map<UserWithRolesDto>(userEntity);

                return CreatedAtRoute("GetUser", new
                { subscriberId = createdUserToReturn.SubscriberId }, createdUserToReturn);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException innerException && innerException.Number == 2601)
                {
                    return BadRequest("Subscriber ID already exists - Duplicate Subscriber ID");
                }
                else
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateUser([FromBody] UserForUpdateDto userForUpdateDto)
        {
            if (userForUpdateDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentifier = _registrationAuthorizationService.GetCurrentUserIdentifier(User);
            if (userIdentifier == null || userIdentifier != userForUpdateDto.SubscriberId)
            {
                return BadRequest();
            }

            var userEntity = _registrationRepository.GetUser(userForUpdateDto.SubscriberId);
            if (userEntity == null)
            {
                return NotFound();
            }

            try
            {
                Mapper.Map(userForUpdateDto, userEntity);

                if (!_registrationRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException innerException && innerException.Number == 2601)
                {
                    return BadRequest("Subscriber ID already exists - Duplicate Subscriber ID");
                }
                else
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [Authorize]
        [HttpPatch]
        public IActionResult PartiallyUpdateUser([FromBody] JsonPatchDocument<UserForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var userIdentifier = _registrationAuthorizationService.GetCurrentUserIdentifier(User);
            if (userIdentifier == null)
            {
                return BadRequest();
            }

            var userEntity = _registrationRepository.GetUser(userIdentifier);
            if (userEntity == null)
            {
                return NotFound();
            }

            var userToPatch = Mapper.Map<UserForUpdateDto>(userEntity);

            patchDoc.ApplyTo(userToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userIdentifier != userToPatch.SubscriberId)
            {
                return BadRequest();
            }

            try
            {
                Mapper.Map(userToPatch, userEntity);

                if (!_registrationRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException innerException && innerException.Number == 2601)
                {
                    return BadRequest("Subscriber ID already exists - Duplicate Subscriber ID");
                }
                else
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }
    }
}
