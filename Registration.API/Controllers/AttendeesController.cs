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
    [Route("api/groups")]
    public class AttendeesController : Controller
    {
        private IRegistrationRepository _registrationRepository;
        private IRegistrationAuthorizationService _registrationAuthorizationService;

        public AttendeesController(IRegistrationRepository registrationRepository, IRegistrationAuthorizationService registrationAuthorizationService)
        {
            _registrationRepository = registrationRepository;
            _registrationAuthorizationService = registrationAuthorizationService;
        }

        [Authorize(Policy = "User")]
        [HttpGet("{groupId}/subgroups/{subgroupId}/attendeestubs", Name = "GetAttendeeStubs")]
        public IActionResult GetAttendeeStubs(int groupId, int subgroupId)
        {
            var userIdentifier = _registrationAuthorizationService.GetCurrentUserIdentifier(User);
            if (userIdentifier == null)
            {
                return BadRequest();
            }

            if(!_registrationAuthorizationService.IsAuthorized(userIdentifier, subgroupId))
            {
                return Unauthorized();
            }

            var attendeeEntities = _registrationRepository.GetAttendees(subgroupId);
            var attendeeStubDtos = Mapper.Map<IEnumerable<AttendeeStubDto>>(attendeeEntities);

            return Ok(attendeeStubDtos);
        }

        [Authorize(Policy = "User")]
        [HttpGet("{groupId}/subgroups/{subgroupId}/attendees", Name = "GetAttendees")]
        public IActionResult GetAttendees(int groupId, int subgroupId)
        {
            var userIdentifier = _registrationAuthorizationService.GetCurrentUserIdentifier(User);
            if (userIdentifier == null)
            {
                return BadRequest();
            }

            if (!_registrationAuthorizationService.IsAuthorized(userIdentifier, subgroupId))
            {
                return Unauthorized();
            }

            var attendeeEntities = _registrationRepository.GetAttendees(subgroupId);
            var attendeeDtos = Mapper.Map<IEnumerable<AttendeeDto>>(attendeeEntities);

            return Ok(attendeeDtos);
        }

        [Authorize(Policy = "User")]
        [HttpGet("{groupId}/subgroups/{subgroupId}/attendees/{attendeeId}", Name = "GetAttendee")]
        public IActionResult GetAttendee(int groupId, int subgroupId, int attendeeId)
        {
            var userIdentifier = _registrationAuthorizationService.GetCurrentUserIdentifier(User);
            if (userIdentifier == null)
            {
                return BadRequest();
            }

            if (!_registrationAuthorizationService.IsAuthorized(userIdentifier, subgroupId))
            {
                return Unauthorized();
            }

            var attendeeEntity = _registrationRepository.GetAttendee(subgroupId, attendeeId);
            var attendeeDto = Mapper.Map<AttendeeDto>(attendeeEntity);

            return Ok(attendeeDto);
        }

        [Authorize(Policy = "User")]
        [HttpPost("{groupId}/subgroups/{subgroupId}/attendees", Name = "CreateAttendee")]
        public IActionResult CreateAttendee(int groupId, int subgroupId, [FromBody] AttendeeForCreationDto attendeeForCreationDto)
        {
            if (attendeeForCreationDto == null || attendeeForCreationDto.SubgroupId != subgroupId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentifier = _registrationAuthorizationService.GetCurrentUserIdentifier(User);
            if (userIdentifier == null)
            {
                return BadRequest();
            }

            if (!_registrationAuthorizationService.IsAuthorized(userIdentifier, subgroupId))
            {
                return Unauthorized();
            }

            if (!(_registrationRepository.GroupExists(groupId) && _registrationRepository.SubgroupExists(groupId, subgroupId)))
            {
                return NotFound();
            }

            var attendeeEntity = Mapper.Map<Entities.Attendee>(attendeeForCreationDto);

            var shirtSize = _registrationRepository.GetShirtSizes().FirstOrDefault(ss => ss.Size == attendeeForCreationDto.ShirtSize);
            attendeeEntity.ShirtSize = shirtSize;

            var attendance = _registrationRepository.GetAttendance(attendeeForCreationDto.Attendance);
            attendeeEntity.Attendance = attendance;

            var user = _registrationRepository.GetUser(userIdentifier);
            attendeeEntity.InsertedById = user.Id;
            attendeeEntity.InsertedOn = DateTime.Now;

            _registrationRepository.AddAttendee(attendeeEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            // Load all Accomodations and Merit Badges
            attendeeEntity = _registrationRepository.GetAttendee(attendeeEntity.SubgroupId, attendeeEntity.Id);

            var createdAttendeeToReturn = Mapper.Map<AttendeeDto>(attendeeEntity);

            return CreatedAtRoute("GetAttendee", new
            { groupId = groupId, subgroupId = subgroupId, attendeeId = createdAttendeeToReturn.Id }, createdAttendeeToReturn);
        }

        [Authorize(Policy = "User")]
        [HttpPut("{groupId}/subgroups/{subgroupId}/attendees/{attendeeId}", Name = "UpdateAttendee")]
        public IActionResult UpdateAttendee(int groupId, int subgroupId, int attendeeId, [FromBody] AttendeeForUpdateDto attendeeForUpdateDto)
        {
            if (attendeeForUpdateDto == null || attendeeForUpdateDto.SubgroupId != subgroupId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentifier = _registrationAuthorizationService.GetCurrentUserIdentifier(User);
            if (userIdentifier == null)
            {
                return BadRequest();
            }

            if (!_registrationAuthorizationService.IsAuthorized(userIdentifier, subgroupId))
            {
                return Unauthorized();
            }

            var attendeeEntity = _registrationRepository.GetAttendee(subgroupId, attendeeId);
            if (attendeeEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(attendeeForUpdateDto, attendeeEntity);

            var shirtSize = _registrationRepository.GetShirtSizes().FirstOrDefault(ss => ss.Size == attendeeForUpdateDto.ShirtSize);
            attendeeEntity.ShirtSize = shirtSize;

            var attendance = _registrationRepository.GetAttendance(attendeeForUpdateDto.Attendance);
            attendeeEntity.Attendance = attendance;

            _registrationRepository.RemoveAllAccommodations(attendeeEntity);

            var attendeeAccommodations = new List<AttendeeAccommodation>();
            foreach (var accommodationDto in attendeeForUpdateDto.Accommodations)
            {
                attendeeAccommodations.Add(new AttendeeAccommodation
                {
                    AttendeeId = attendeeId,
                    AccommodationId = accommodationDto.Id
                });
            }

            attendeeEntity.AttendeeAccommodations = attendeeAccommodations;

            _registrationRepository.RemoveAllMeritBadges(attendeeEntity);

            var attendeeMeritBadges = new List<AttendeeMeritBadge>();
            var sortOrder = 1;
            foreach (var meritBadgeForCreationDto in attendeeForUpdateDto.MeritBadges)
            {
                attendeeMeritBadges.Add(new AttendeeMeritBadge
                {
                    AttendeeId = attendeeId,
                    MeritBadgeId = meritBadgeForCreationDto.Id,
                    SortOrder = sortOrder++
                });
            }

            attendeeEntity.AttendeeMeritBadges = attendeeMeritBadges;

            var user = _registrationRepository.GetUser(userIdentifier);
            attendeeEntity.UpdatedById = user.Id;
            attendeeEntity.UpdatedOn = DateTime.Now;

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
