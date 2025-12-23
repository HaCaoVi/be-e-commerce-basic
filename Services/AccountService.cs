using e_commerce_basic.Dtos.Account;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_basic.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<NewUserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.UserName == loginDto.Username.ToLower()
                )
                ?? throw new UnauthorizedAccessException("Username or password is invalid");

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Username or password is invalid");

            if (!user.EmailConfirmed)
                throw new UnauthorizedAccessException("Email not confirmed");

            if (!user.IsActivated)
                throw new UnauthorizedAccessException("Account is disabled");

            return new NewUserDto
            {
                Email = user.Email!,
                Username = user.UserName!,
                AccessToken = ""
            };
        }

    }
}