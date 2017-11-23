using Microsoft.AspNetCore.Mvc;
using Registration.API.Models;
using Registration.API.Models.Contacts;
using Registration.API.Services.Email;
using System;
using System.Threading.Tasks;

namespace Registration.API.Controllers
{
    [Route("api/contacts")]
    public class ContactController : Controller
    {
        private IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] ContactDto contactDto)
        {
            if (contactDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                EmailMessage email = _emailService.GenerateEmailMessage(contactDto);

                var response = await _emailService.SendMessage(email);

                return StatusCode((int)response.StatusCode, response.StatusCode.ToString());
            }
            catch(Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
