using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Entities;
using Registration.API.Models;
using Registration.API.Services;
using System.Collections.Generic;
using System.Linq;

namespace Registration.API.Controllers
{
    [Route("api/groups")]
    public class AttendeeAccommodationsController : Controller
    {
        private IRegistrationRepository _registrationRepository;

        public AttendeeAccommodationsController(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        [Authorize(Policy = "User")]
        [HttpGet("{groupId}/subgroups/{subgroupId}/attendees/{attendeeId}/accommodations", Name = "GetAccommodations")]
        public IActionResult GetAccommodations(int groupId, int subgroupId, int attendeeId)
        {
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
