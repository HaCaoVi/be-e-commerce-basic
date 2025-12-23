using System.ComponentModel.DataAnnotations;
using e_commerce_basic.Types;
using Microsoft.AspNetCore.Identity;

namespace e_commerce_basic.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(255)]
        public required string Fullname { get; set; }
        [MaxLength(500)]
        public string AvatarUrl { get; set; } = string.Empty;
        public DateTime? Birthday { get; set; }
        public EGenderType Gender { get; set; } = EGenderType.Unknown;
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;
        public EAccountType AccountType { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsActivated { get; set; } = false;
        [MinLength(4)]
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}