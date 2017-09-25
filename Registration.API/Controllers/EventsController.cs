using AutoMapper;
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

        [HttpGet]
        public IActionResult GetEvents()
        {
            var eventEntities = _registrationRepository.GetEvents();
            var eventDtos = Mapper.Map<IEnumerable<EventDto>>(eventEntities);

            return Ok(eventDtos);
        }
    }
}
