using e_commerce_basic.Common;
using e_commerce_basic.Dtos.Account;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Models;
using e_commerce_basic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _accountService.LoginAsync(loginDto);
            return Ok(ApiResponse<NewUserDto>.Ok(result));
        }
    }
}