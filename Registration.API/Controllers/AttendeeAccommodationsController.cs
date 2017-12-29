using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Entities;
using Registration.API.Models;
using Registration.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Registration.API.Controllers
{
    [Route("api/groups")]
    public class AttendeeAccommodationsController : Controller
    {
        private IRegistrationRepository _registrationRepository;
        private IRegistrationAuthorizationService _registrationAuthorizationService;

        public AttendeeAccommodationsController(IRegistrationRepository registrationRepository, IRegistrationAuthorizationService registrationAuthorizationService)
        {
            _registrationRepository = registrationRepository;
            _registrationAuthorizationService = registrationAuthorizationService;
        }

        [Authorize(Policy = "User")]
        [HttpGet("{groupId}/subgroups/{subgroupId}/attendees/{attendeeId}/accommodations", Name = "GetAccommodations")]
        public IActionResult GetAccommodations(int groupId, int subgroupId, int attendeeId)
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

            if (attendeeEntity == null)
            {
                return NotFound();
            }

            var accommodationDto = new List<AccommodationDto>();
            foreach (var attendeeAccommodation in attendeeEntity.AttendeeAccommodations)
            {
                var accommodation = _registrationRepository.GetAccommodations().FirstOrDefault(a => a.Id == attendeeAccommodation.AccommodationId);
                accommodationDto.Add(Mapper.Map<AccommodationDto>(accommodation));
            }

            return Ok(accommodationDto);
        }

        [Authorize(Policy = "User")]
        [HttpPost("{groupId}/subgroups/{subgroupId}/attendees/{attendeeId}/accommodations", Name = "PostAccommodations")]
        public IActionResult PostAccommodations(int groupId, int subgroupId, int attendeeId,
            [FromBody] IEnumerable<AccommodationDto> accommodationDtos)
        {
            if (accommodationDtos == null)
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

            _registrationRepository.RemoveAllAccommodations(attendeeEntity);

            var attendeeAccommodations = new List<AttendeeAccommodation>();
            foreach (var accommodationDto in accommodationDtos)
            {
                attendeeAccommodations.Add(new AttendeeAccommodation
                {
                    AttendeeId = attendeeId,
                    AccommodationId = accommodationDto.Id
                });
            }

            attendeeEntity.AttendeeAccommodations = attendeeAccommodations;

            var user = _registrationRepository.GetUser(userIdentifier);
            attendeeEntity.UpdatedById = user.Id;
            attendeeEntity.UpdatedOn = DateTime.Now;
            
            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdAccommodationsToReturn = new List<AccommodationDto>();
            var attendee = _registrationRepository.GetAttendee(subgroupId, attendeeId);
            foreach (var attendeeAccommodation in attendee.AttendeeAccommodations)
            {
                createdAccommodationsToReturn.Add(new AccommodationDto
                {
                    Id = attendeeAccommodation.Accommodation.Id,
                    Name = attendeeAccommodation.Accommodation.Name
                });
            }

            return CreatedAtRoute("GetAccommodations", new
            { groupId = groupId, subgroupId = subgroupId, attendeeId = attendeeId }, createdAccommodationsToReturn);
        }
    }
}
