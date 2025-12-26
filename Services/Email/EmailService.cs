using SendGrid;
using SendGrid.Helpers.Mail;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Models;

namespace e_commerce_basic.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(EmailMessage message)
        {
            var client = new SendGridClient(_config["SendGrid:ApiKey"]);

            var from = new EmailAddress(
                _config["SendGrid:FromEmail"],
                _config["SendGrid:FromName"]
            );

            var to = new EmailAddress(message.To);

            var msg = MailHelper.CreateSingleTemplateEmail(
                from,
                to,
                message.TemplateId,
                message.Data
            );

            var response = await client.SendEmailAsync(msg);

            var body = await response.Body.ReadAsStringAsync();

            Console.WriteLine($"SendGrid Status: {response.StatusCode}");
            Console.WriteLine(body);
        }
    }
}
