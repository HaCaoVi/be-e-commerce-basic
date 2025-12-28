using System.Security.Claims;
using e_commerce_basic.Common;
using e_commerce_basic.Dtos.Account;
using e_commerce_basic.Dtos.Token;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Mappings;
using e_commerce_basic.Models;
using e_commerce_basic.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_basic.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly EmailConfirmationService _emailConfirmationService;

        public AccountController(IAccountService accountService, EmailConfirmationService emailConfirmationService)
        {
            _accountService = accountService;
            _emailConfirmationService = emailConfirmationService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var user = await _accountService.RegisterAsync(registerDto);
            await _emailConfirmationService.SendConfirmEmailAsync(user);
            return Ok(ApiResponse<AccountDto>.Ok(user.ToAccountDto("User")));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _accountService.LoginAsync(loginDto);
            return Ok(ApiResponse<TokenDto>.Ok(result));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var user = HttpContext.User;
            var username = user.FindFirst(ClaimTypes.GivenName)?.Value;
            await _accountService.LogoutAsync(username!);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAccount()
        {
            var user = HttpContext.User;
            var username = user.FindFirst(ClaimTypes.GivenName)?.Value;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            var Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(ApiResponse<NewTokenDto>.Ok(new NewTokenDto
            {
                RoleName = role!,
                Username = username!,
                Id = Id!.ToString()
            }
           ));
        }
    }
}