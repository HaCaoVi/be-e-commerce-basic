using System.ComponentModel.DataAnnotations;

namespace e_commerce_basic.Dtos.Account
{
    public class LoginDto
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}