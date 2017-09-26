using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Models;
using Registration.API.Services;
using System;
using System.Collections.Generic;

namespace Registration.API.Controllers
{
    [Route("api/groups")]
    public class SubgroupsController : Controller
    {
        private IRegistrationRepository _registrationRepository;

        public SubgroupsController(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        [HttpGet("{groupId}/subgroups")]
        public IActionResult GetSubgroups(int groupId)
        {
            try
            {
                if (!_registrationRepository.GroupExists(groupId))
                {
                    return NotFound();
                }

                var subgroupEntities = _registrationRepository.GetSubgroups(groupId);
                var subgroupDtos = Mapper.Map<IEnumerable<SubgroupDto>>(subgroupEntities);

                return Ok(subgroupDtos);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }
        
        [HttpGet("{groupId}/subgroups/{id}", Name = "GetSubgroup")]
        public IActionResult GetSubgroup(int groupId, int id)
        {
            if (!_registrationRepository.GroupExists(groupId))
            {
                return NotFound();
            }

            var subgroupEntities = _registrationRepository.GetSubgroup(groupId, id);

            if (subgroupEntities == null)
            {
                return NotFound();
            }

            var subgroupResult = Mapper.Map<SubgroupDto>(subgroupEntities);
            return Ok(subgroupResult);
        }

        [HttpPost("{groupId}/subgroups")]
        public IActionResult CreateSubgroup(int groupId, [FromBody] SubgroupForCreationDto subgroupDto)
        {
            if (subgroupDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_registrationRepository.GroupExists(groupId))
            {
                return NotFound();
            }

            var subgroupEntity = Mapper.Map<Entities.Subgroup>(subgroupDto);

            _registrationRepository.AddSubgroup(groupId, subgroupEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdSubgroupToReturn = Mapper.Map<SubgroupDto>(subgroupEntity);

            return CreatedAtRoute("GetSubgroup", new
            { id = createdSubgroupToReturn.Id }, createdSubgroupToReturn);
        }

        [HttpPut("{groupId}/subgroups/{id}")]
        public IActionResult UpdateSubgroup(int groupId, int id,
            [FromBody] SubgroupForUpdateDto subgroupDto)
        {
            if (subgroupDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_registrationRepository.GroupExists(groupId))
            {
                return NotFound();
            }

            var subgroupEntity = _registrationRepository.GetSubgroup(groupId, id);
            if (subgroupEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(subgroupDto, subgroupEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpPatch("{groupId}/subgroups/{id}")]
        public IActionResult PartiallyUpdateSubgroup(int groupId, int id,
            [FromBody] JsonPatchDocument<SubgroupForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_registrationRepository.GroupExists(groupId))
            {
                return NotFound();
            }

            var subgroupEntity = _registrationRepository.GetSubgroup(groupId, id);
            if (subgroupEntity == null)
            {
                return NotFound();
            }

            var subgroupToPatch = Mapper.Map<SubgroupForUpdateDto>(subgroupEntity);

            patchDoc.ApplyTo(subgroupToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(subgroupToPatch, subgroupEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete("{groupId}/subgroups/{id}")]
        public IActionResult DeleteSubgroup(int groupId, int id)
        {
            if (!_registrationRepository.GroupExists(groupId))
            {
                return NotFound();
            }

            var subgroupEntity = _registrationRepository.GetSubgroup(groupId, id);
            if (subgroupEntity == null)
            {
                return NotFound();
            }

            _registrationRepository.DeleteSubgroup(subgroupEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
