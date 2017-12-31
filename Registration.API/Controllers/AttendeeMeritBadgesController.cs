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
    public class AttendeeMeritBadgesController : Controller
    {
        private IRegistrationRepository _registrationRepository;
        private IRegistrationAuthorizationService _registrationAuthorizationService;

        public AttendeeMeritBadgesController(IRegistrationRepository registrationRepository, IRegistrationAuthorizationService registrationAuthorizationService)
        {
            _registrationRepository = registrationRepository;
            _registrationAuthorizationService = registrationAuthorizationService;
        }

        [Authorize(Policy = "User")]
        [HttpGet("{groupId}/subgroups/{subgroupId}/attendees/{attendeeId}/meritbadges", Name = "GetMeritBadges")]
        public IActionResult GetMeritBadges(int groupId, int subgroupId, int attendeeId)
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

            var meritBadgeDtos = new List<MeritBadgeDto>();
            foreach (var attendeeMeritBadge in attendeeEntity.AttendeeMeritBadges)
            {
                var meritBadge = _registrationRepository.GetMeritBadges().FirstOrDefault(mb => mb.Id == attendeeMeritBadge.MeritBadgeId);
                var meritBadgeDto = Mapper.Map<MeritBadgeDto>(meritBadge);
                meritBadgeDto.SortOrder = attendeeMeritBadge.SortOrder;
                meritBadgeDtos.Add(meritBadgeDto);
            }

            return Ok(meritBadgeDtos);
        }

        [Authorize(Policy = "User")]
        [HttpPost("{groupId}/subgroups/{subgroupId}/attendees/{attendeeId}/meritbadges", Name = "PostMeritBadges")]
        public IActionResult PostMeritBadges(int groupId, int subgroupId, int attendeeId,
            [FromBody] IEnumerable<MeritBadgeForCreationDto> meritBadgeForCreationDtos)
        {
            if (meritBadgeForCreationDtos == null)
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

            _registrationRepository.RemoveAllMeritBadges(attendeeEntity);

            var attendeeMeritBadges = new List<AttendeeMeritBadge>();
            var sortOrder = 1;
            foreach (var meritBadgeForCreationDto in meritBadgeForCreationDtos)
            {
                attendeeMeritBadges.Add(new AttendeeMeritBadge {
                    AttendeeId = attendeeId,
                    MeritBadgeId = meritBadgeForCreationDto.Id,
                    SortOrder = sortOrder++
                });
            }

            attendeeEntity.AttendeeMeritBadges = attendeeMeritBadges;

            var user = _registrationRepository.GetUser(userIdentifier);
            attendeeEntity.UpdatedById = user.Id;
            attendeeEntity.UpdatedOn = DateTime.UtcNow;

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdMeritBadgesToReturn = new List<MeritBadgeDto>();
            var attendee = _registrationRepository.GetAttendee(subgroupId, attendeeId);
            foreach (var attendeeMeritBadge in attendee.AttendeeMeritBadges)
            {
                createdMeritBadgesToReturn.Add(new MeritBadgeDto
                {
                    Id = attendeeMeritBadge.MeritBadge.Id,
                    Name = attendeeMeritBadge.MeritBadge.Name,
                    SortOrder = attendeeMeritBadge.SortOrder
                });
            }

            return CreatedAtRoute("GetMeritBadges", new
            { groupId = groupId, subgroupId = subgroupId, attendeeId = attendeeId }, createdMeritBadgesToReturn);
        }
    }
}
