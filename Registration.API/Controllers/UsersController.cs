using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Models;
using Registration.API.Services;
using System.Collections.Generic;
using Registration.API.Entities;
using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Registration.API.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private IRegistrationRepository _registrationRepository;

        public UsersController(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var userEntities = _registrationRepository.GetUsers();
            var userDtos = Mapper.Map<IEnumerable<UserDto>>(userEntities);

            return Ok(userDtos);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("{subscriberId}", Name = "GetUser")]
        public IActionResult GetUser(string subscriberId, bool includeRoles = false)
        {
            var user = _registrationRepository.GetUser(subscriberId, includeRoles);

            if (user == null)
            {
                return NotFound();
            }

            if (includeRoles)
            {
                var userWithRolesResult = Mapper.Map<UserWithRolesDto>(user);
                return Ok(userWithRolesResult);
            }

            var userResult = Mapper.Map<UserDto>(user);
            return Ok(userResult);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserForCreationDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userEntity = Mapper.Map<User>(userDto);

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
            catch(Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("{subscriberId}", Name = "UpdateUser")]
        public IActionResult UpdateUser(string subscriberId, [FromBody] UserForUpdateDto userForUpdateDto)
        {
            if (userForUpdateDto == null || userForUpdateDto.SubscriberId != subscriberId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

        [Authorize(Policy = "Admin")]
        [HttpPatch("{subscriberId}", Name = "PartiallyUpdateUser")]
        public IActionResult PartiallyUpdateUser(string subscriberId, [FromBody] JsonPatchDocument<UserForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var userEntity = _registrationRepository.GetUser(subscriberId);
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

        [Authorize(Policy = "Admin")]
        [HttpDelete("{subscriberId}", Name = "DeleteUser")]
        public IActionResult DeleteUser(string subscriberId)
        {
            var userEntity = _registrationRepository.GetUser(subscriberId);
            if (userEntity == null)
            {
                return NotFound();
            }

            _registrationRepository.DeleteUser(userEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
