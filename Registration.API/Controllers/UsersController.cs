using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Models;
using Registration.API.Services;
using System.Collections.Generic;
using Registration.API.Entities;

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

        [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var userEntities = _registrationRepository.GetUsers();
            var userDtos = Mapper.Map<IEnumerable<UserDto>>(userEntities);

            return Ok(userDtos);
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetUser")]
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

        [Authorize]
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

            var userExists = _registrationRepository.UserExists(userDto.SubscriberId);
            User userEntity;

            if (userExists)
            {
                userEntity = _registrationRepository.GetUser(userDto.SubscriberId, false);
                Mapper.Map(userDto, userEntity);
            }
            else
            {
                userEntity = Mapper.Map<User>(userDto);
                _registrationRepository.AddUser(userEntity);
            }

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdUserToReturn = Mapper.Map<UserDto>(userEntity);

            return CreatedAtRoute("GetUser", new
            { id = createdUserToReturn.SubscriberId }, createdUserToReturn);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserForUpdateDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEntity = _registrationRepository.GetUser(id, includeRoles: false);
            if (userEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(userDto, userEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [Authorize]
        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateUser(int id, [FromBody] JsonPatchDocument<UserForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var userEntity = _registrationRepository.GetUser(id, includeRoles: false);
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

            Mapper.Map(userToPatch, userEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var userEntity = _registrationRepository.GetUser(id, includeRoles: false);
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
