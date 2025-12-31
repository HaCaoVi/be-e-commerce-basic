using System.ComponentModel.DataAnnotations;
using e_commerce_basic.Common.Validators;
using e_commerce_basic.Dtos.Gallery;
using e_commerce_basic.Types;

namespace e_commerce_basic.Dtos.Product
{
    public class UpdateProductDto
    {
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 3)]
        public string Code { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(0.01, 1_000_000_000_000)]
        public decimal Price { get; set; }

        [DiscountValidator]
        public decimal Discount { get; set; }

        [EnumDataType(typeof(EDiscount))]
        public EDiscount TypeDiscount { get; set; }

        public bool IsActivated { get; set; } = true;

        [Range(1, int.MaxValue)]
        public int SubCategoryId { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [MinLength(1, ErrorMessage = "At least one gallery image is required")]
        public List<CreateGalleryDto> Galleries { get; set; } = [];
    }
}