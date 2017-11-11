using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Models;
using Registration.API.Services;
using System.Collections.Generic;

namespace Registration.API.Controllers
{
    [Route("api/groups")]
    public class GroupsController : Controller
    {
        private IRegistrationRepository _registrationRepository;

        public GroupsController(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetGroups()
        {
            var groupEntities = _registrationRepository.GetGroups();
            var groupDtos = Mapper.Map<IEnumerable<GroupDto>>(groupEntities);

            return Ok(groupDtos);
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetGroup")]
        public IActionResult GetGroup(int id, bool includeSubgroups = false)
        {
            var group = _registrationRepository.GetGroup(id, includeSubgroups);

            if (group == null)
            {
                return NotFound();
            }

            if (includeSubgroups)
            {
                var groupWithSubgroupsResult = Mapper.Map<GroupWithSubgroupsDto>(group);
                return Ok(groupWithSubgroupsResult);
            }

            var groupResult = Mapper.Map<GroupDto>(group);
            return Ok(groupResult);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateGroup([FromBody] GroupForCreationDto groupDto)
        {
            if (groupDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groupEntity = Mapper.Map<Entities.Group>(groupDto);

            _registrationRepository.AddGroup(groupEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdGroupToReturn = Mapper.Map<GroupDto>(groupEntity);

            return CreatedAtRoute("GetGroup", new
            { id = createdGroupToReturn.Id }, createdGroupToReturn);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateGroup(int id,
            [FromBody] GroupForUpdateDto groupDto)
        {
            if (groupDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groupEntity = _registrationRepository.GetGroup(id, includeSubgroups: false);
            if (groupEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(groupDto, groupEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [Authorize]
        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateGroup(int id,
            [FromBody] JsonPatchDocument<GroupForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var groupEntity = _registrationRepository.GetGroup(id, includeSubgroups: false);
            if (groupEntity == null)
            {
                return NotFound();
            }

            var groupToPatch = Mapper.Map<GroupForUpdateDto>(groupEntity);

            patchDoc.ApplyTo(groupToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(groupToPatch, groupEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteGroup(int id)
        {
            var groupEntity = _registrationRepository.GetGroup(id, includeSubgroups: false);
            if (groupEntity == null)
            {
                return NotFound();
            }

            _registrationRepository.DeleteGroup(groupEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
