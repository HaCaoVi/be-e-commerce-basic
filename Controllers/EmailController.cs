using e_commerce_basic.Interfaces;
using e_commerce_basic.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_basic.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {
        private readonly EmailConfirmationService _emailConfirmationService;

        public EmailController(EmailConfirmationService emailConfirmationService)
        {
            _emailConfirmationService = emailConfirmationService;
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            await _emailConfirmationService.ConfirmEmailAsync(userId, token);
            return Ok("Email confirmed");
        }
    }
}