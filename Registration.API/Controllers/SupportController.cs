using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Models;
using Registration.API.Services;
using System.Collections.Generic;

namespace Registration.API.Controllers
{
    [Route("api/support")]
    public class SupportController : Controller
    {
        private IRegistrationRepository _registrationRepository;

        public SupportController(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        [Authorize(Policy = "User")]
        [HttpGet("shirtsizes", Name = "GetAllShirtSizes")]
        public IActionResult GetAllShirtSizes()
        {
            var shirtSizeEntities = _registrationRepository.GetShirtSizes();
            var shirtSizeDtos = Mapper.Map<IEnumerable<ShirtSizeDto>>(shirtSizeEntities);

            return Ok(shirtSizeDtos);
        }

        [Authorize(Policy = "User")]
        [HttpGet("meritbadges", Name = "GetAllMeritBadges")]
        public IActionResult GetAllMeritBadges()
        {
            var meritBadgesEntities = _registrationRepository.GetMeritBadges();
            var meritBadgeDtos = Mapper.Map<IEnumerable<MeritBadgeForCreationDto>>(meritBadgesEntities);

            return Ok(meritBadgeDtos);
        }

        [Authorize(Policy = "User")]
        [HttpGet("accommodations", Name = "GetAllAccommodations")]
        public IActionResult GetAllAccommodations()
        {
            var accommodationEntities = _registrationRepository.GetAccommodations();
            var accommodationDtos = Mapper.Map<IEnumerable<AccommodationDto>>(accommodationEntities);

            return Ok(accommodationDtos);
        }
    }
}
