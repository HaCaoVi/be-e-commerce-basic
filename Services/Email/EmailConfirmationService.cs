using Microsoft.AspNetCore.Identity;
using e_commerce_basic.Models;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Common;

namespace e_commerce_basic.Services.Auth
{
    public class EmailConfirmationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public EmailConfirmationService(
            UserManager<User> userManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        // ✅ Confirm email
        public async Task<User> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new BadRequestException("Invalid user");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                throw new BadRequestException("Invalid token");

            return user;
        }

        // ✅ Send confirm email
        public async Task SendConfirmEmailAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmUrl =
                $"https://fe.yourapp.com/confirm-email" +
                $"?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendAsync(new EmailMessage
            {
                To = user.Email!,
                TemplateId = "d-0a61027ef9574fdd8ca1e3876aa12a86",
                Data = new()
                {
                    ["fullname"] = user.Fullname,
                    ["confirm_url"] = confirmUrl
                }
            });
        }
    }
}
