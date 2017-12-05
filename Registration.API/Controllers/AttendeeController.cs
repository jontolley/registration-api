using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Services;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Registration.API.Models;

namespace Registration.API.Controllers
{
    [Route("api/groups")]
    public class AttendeeController : Controller
    {
        private IRegistrationRepository _registrationRepository;

        public AttendeeController(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        [Authorize(Policy = "User")]
        [HttpGet("{groupId}/subgroups/{subgroupId}/attendees", Name = "GetAttendees")]
        public IActionResult GetAttendees(int groupId, int subgroupId)
        {
            var attendeeEntities = _registrationRepository.GetAttendees(subgroupId);
            var attendeeDtos = Mapper.Map<IEnumerable<AttendeeDto>>(attendeeEntities);

            return Ok(attendeeDtos);
        }

        [Authorize(Policy = "User")]
        [HttpGet("{groupId}/subgroups/{subgroupId}/attendees/{attendeeId}", Name = "GetAttendee")]
        public IActionResult GetAttendee(int groupId, int subgroupId, int attendeeId)
        {
            var attendeeEntity = _registrationRepository.GetAttendee(subgroupId, attendeeId);
            var attendeeDto = Mapper.Map<AttendeeDto>(attendeeEntity);

            return Ok(attendeeDto);
        }

        [Authorize(Policy = "User")]
        [HttpPost("{groupId}/subgroups/{subgroupId}/attendees", Name = "CreateAttendee")]
        public IActionResult CreateAttendee(int groupId, int subgroupId, [FromBody] AttendeeForCreationDto attendeeForCreationDto)
        {
            if (attendeeForCreationDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!(_registrationRepository.GroupExists(groupId) && _registrationRepository.SubgroupExists(groupId, subgroupId)))
            {
                return NotFound();
            }

            var attendeeEntity = Mapper.Map<Entities.Attendee>(attendeeForCreationDto);

            _registrationRepository.AddAttendee(attendeeEntity);

            // TODO: add Merit Badges
            // TODO: add Accommidations

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdAttendeeToReturn = Mapper.Map<AttendeeDto>(attendeeEntity);
            
            return CreatedAtRoute("GetAttendee", new
            { groupId = groupId, subgroupId = subgroupId, attendeeId = createdAttendeeToReturn.Id }, createdAttendeeToReturn);
        }

        //// POST: api/Attendee
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Attendee/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
