using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using e_commerce_basic.Types;

namespace e_commerce_basic.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Description { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }
        public EDiscount TypeDiscount { get; set; }
        public bool IsActivated { get; set; } = true;
        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
        public required SubCategory SubCategory { get; set; }
        public Stock? Stock { get; set; }
        public List<Gallery> Galleries { get; set; } = [];
    }
}