using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Models;
using Registration.API.Services;
using System.Collections.Generic;

namespace Registration.API.Controllers
{
    [Route("api/events")]
    public class EventsController : Controller
    {
        private IRegistrationRepository _registrationRepository;

        public EventsController(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }
        
        [Authorize]
        [HttpGet]
        public IActionResult GetEvents()
        {
            var x = User;

            var y = HttpContext.User;

            var eventEntities = _registrationRepository.GetEvents();
            var eventDtos = Mapper.Map<IEnumerable<EventDto>>(eventEntities);

            return Ok(eventDtos);
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetEvent")]
        public IActionResult GetEvent(int id)
        {
            var event_ = _registrationRepository.GetEvent(id);

            if (event_ == null)
            {
                return NotFound();
            }

            var eventResult = Mapper.Map<EventDto>(event_);
            return Ok(eventResult);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateEvent([FromBody] EventForCreationDto eventDto)
        {
            if (eventDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eventEntity = Mapper.Map<Entities.Event>(eventDto);

            _registrationRepository.AddEvent(eventEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdEventToReturn = Mapper.Map<EventDto>(eventEntity);

            return CreatedAtRoute("GetEvent", new
            { id = createdEventToReturn.Id }, createdEventToReturn);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateEvent(int id,
            [FromBody] EventForUpdateDto eventDto)
        {
            if (eventDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eventEntity = _registrationRepository.GetEvent(id);
            if (eventEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(eventDto, eventEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [Authorize]
        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateEvent(int id,
            [FromBody] JsonPatchDocument<EventForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var eventEntity = _registrationRepository.GetEvent(id);
            if (eventEntity == null)
            {
                return NotFound();
            }

            var eventToPatch = Mapper.Map<EventForUpdateDto>(eventEntity);

            patchDoc.ApplyTo(eventToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(eventToPatch, eventEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var eventEntity = _registrationRepository.GetEvent(id);
            if (eventEntity == null)
            {
                return NotFound();
            }

            _registrationRepository.DeleteEvent(eventEntity);

            if (!_registrationRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
