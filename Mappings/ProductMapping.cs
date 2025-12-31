using e_commerce_basic.Dtos.Gallery;
using e_commerce_basic.Dtos.Product;
using e_commerce_basic.Dtos.Stock;
using e_commerce_basic.Dtos.SubCategory;
using e_commerce_basic.Models;

namespace e_commerce_basic.Mappings
{
    public static class ProductMapping
    {
        public static ProductDto ToProductDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                Description = product.Description,
                CreatedAt = product.CreatedAt,
                Discount = product.Discount,
                IsActivated = product.IsActivated,
                IsDeleted = product.IsDeleted,
                Price = product.Price,
                TypeDiscount = product.TypeDiscount,
                UpdatedAt = product.UpdatedAt,
                Stock = product.Stock == null ? null : new StockDto
                {
                    Quantity = product.Stock.Quantity,
                    Sold = product.Stock.Sold,
                },
                Galleries = product.Galleries?.Select(g => new GalleryDto
                {
                    Id = g.Id,
                    Url = g.Url,
                    ContentType = g.ContentType,
                    FileName = g.FileName,
                    Size = g.Size
                }).ToList() ?? [],
            };
        }

        public static Product ToProductEntity(this CreateProductDto createProductDto)
        {
            return new Product
            {
                Name = createProductDto.Name,
                Code = createProductDto.Code,
                Description = createProductDto.Description,
                Discount = createProductDto.Discount,
                IsActivated = createProductDto.IsActivated,
                Price = createProductDto.Price,
                TypeDiscount = createProductDto.TypeDiscount,
                SubCategoryId = createProductDto.SubCategoryId,
            };
        }
    }
}