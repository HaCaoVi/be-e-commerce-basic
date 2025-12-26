using e_commerce_basic.Dtos.Account;
using e_commerce_basic.Dtos.Token;
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
        private readonly ITokenService _tokenService;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var normalizedUsername = _userManager.NormalizeName(loginDto.Username);
            var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUsername
                )
                ?? throw new UnauthorizedAccessException("Username or password is invalid");

            if (!user.EmailConfirmed)
                throw new UnauthorizedAccessException("Email not confirmed");

            if (!user.IsActivated)
                throw new UnauthorizedAccessException("Account is disabled");

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Username or password is invalid");

            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? throw new InvalidOperationException("User has no role");

            var email = user.Email ?? throw new InvalidOperationException("User email is null");
            var fullname = user.Fullname ?? throw new InvalidOperationException("User fullname is null");
            var username = user.UserName ?? throw new InvalidOperationException("User username is null");
            var newTokenDto = new NewTokenDto
            {
                Email = email,
                Id = int.Parse(user.Id),
                RoleName = roleName,
                Username = username
            };
            user.RefreshToken = _tokenService.CreateRefreshToken(newTokenDto);
            await _userManager.UpdateAsync(user);
            return new TokenDto
            {
                Email = email,
                Fullname = fullname,
                AccessToken = _tokenService.CreateAccessToken(newTokenDto),
            };
        }

        public async Task<User> LogoutAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new BadHttpRequestException("UserId not found");
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return user;
        }
    }
}