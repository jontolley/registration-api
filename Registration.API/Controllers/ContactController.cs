using Microsoft.AspNetCore.Mvc;
using Registration.API.Models;
using Registration.API.Models.Contacts;
using Registration.API.Services.Email;
using System;
using System.Threading.Tasks;

namespace Registration.API.Controllers
{
    [Route("api/contact")]
    public class ContactController : Controller
    {
        private IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] ContactDto contactDto)
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
                EmailMessage email = await _emailService.GenerateEmailMessageAsync(contactDto);

                var response = await _emailService.SendMessage(email);
                
                return StatusCode((int)response.StatusCode);
            }
            catch(Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
