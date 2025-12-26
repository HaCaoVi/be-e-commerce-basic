using e_commerce_basic.Models;

namespace e_commerce_basic.Interfaces
{
    public interface IEmailService
    {
        // Task<User> ConfirmEmailAsync(string userId, string token);
        Task SendAsync(EmailMessage emailMessage);
    }
}