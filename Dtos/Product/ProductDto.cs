using e_commerce_basic.Dtos.Gallery;
using e_commerce_basic.Dtos.Stock;
using e_commerce_basic.Dtos.SubCategory;
using e_commerce_basic.Models;
using e_commerce_basic.Types;

namespace e_commerce_basic.Dtos.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public EDiscount TypeDiscount { get; set; }
        public bool IsActivated { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public StockDto? Stock { get; set; }
        public int SubCategoryId { get; set; }
        public List<GalleryDto> Galleries { get; set; } = [];
    }
}