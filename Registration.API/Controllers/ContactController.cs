using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Models;
using Registration.API.Services;
using Registration.API.Services.Email;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Registration.API.Controllers
{
    [Route("api/contacts")]
    public class ContactController : Controller
    {
        private IRegistrationRepository _registrationRepository;
        private IEmailService _emailService;

        public ContactController(IEmailService emailService, IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
            _emailService = emailService;
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public IActionResult GetContacts()
        {
            var contactEntities = _registrationRepository.GetContacts();
            var contactDtos = Mapper.Map<IEnumerable<ContactDto>>(contactEntities);

            return Ok(contactDtos);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("{id}", Name = "GetContact")]
        public IActionResult GetContact(int id)
        {
            var contact = _registrationRepository.GetContact(id);

            if (contact == null)
            {
                return NotFound();
            }

            var contactDto = Mapper.Map<ContactDto>(contact);
            return Ok(contactDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] ContactForCreationDto contactForCreationDto)
        {
            if (contactForCreationDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var contactEntity = Mapper.Map<Entities.Contact>(contactForCreationDto);

                _registrationRepository.AddContact(contactEntity);

                if (!_registrationRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                var createdContactToReturn = Mapper.Map<ContactDto>(contactEntity);

                var email = await _emailService.GenerateEmailMessageAsync(createdContactToReturn);

                await _emailService.SendMessage(email);

                return CreatedAtRoute("GetContact", new
                { id = createdContactToReturn.Id }, createdContactToReturn);
            }
            catch(Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }
    }
}
