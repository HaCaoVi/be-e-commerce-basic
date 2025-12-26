using System.ComponentModel.DataAnnotations;

namespace e_commerce_basic.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(255)]
        public required string Fullname { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public required string Email { get; set; }
        [Required]
        [MaxLength(255)]
        public required string Username { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(64)]
        public required string Password { get; set; }
    }
}