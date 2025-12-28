
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using e_commerce_basic.Dtos.Gallery;
using e_commerce_basic.Types;

namespace e_commerce_basic.Dtos.Product
{
    public class CreateProductDto
    {
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
        public int SubCategoryId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "At least one gallery image is required")]
        public List<CreateGalleryDto> Galleries { get; set; } = [];
    }
}