using System.Security.Claims;
using e_commerce_basic.Common;
using e_commerce_basic.Dtos.Account;
using e_commerce_basic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_basic.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _accountService.LoginAsync(loginDto);
            return Ok(ApiResponse<TokenDto>.Ok(result));
        }

        [HttpGet()]
        public async Task<IActionResult> GetAccount()
        {
            var user = HttpContext.User;
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            var fullname = user.FindFirst(ClaimTypes.Name)?.Value;
            var username = user.FindFirst(ClaimTypes.GivenName)?.Value;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(ApiResponse<AccountDto>.Ok(new AccountDto
            {
                Email = email!,
                Fullname = fullname!,
                Role = role!,
                Username = username!
            }
           ));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var user = HttpContext.User;
            var username = user.FindFirst(ClaimTypes.GivenName)?.Value;
            await _accountService.LogoutAsync(username!);
            return NoContent();
        }
    }
}