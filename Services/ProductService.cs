using e_commerce_basic.Common;
using e_commerce_basic.Database;
using e_commerce_basic.Dtos.Product;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Mappings;
using e_commerce_basic.Models;

namespace e_commerce_basic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repoProduct;
        private readonly ISubCategoryRepository _repoSubCategory;
        private readonly IStockRepository _repoStock;
        private readonly IGalleryRepository _repoGallery;
        public ProductService(IProductRepository repoProduct, ISubCategoryRepository repoSubCategory, IStockRepository repoStock, IGalleryRepository repoGallery)
        {
            _repoProduct = repoProduct;
            _repoSubCategory = repoSubCategory;
            _repoStock = repoStock;
            _repoGallery = repoGallery;
        }

        public async Task<Product> HandleAddProductAsync(CreateProductDto createProductDto)
        {
            var subIdExist = await _repoSubCategory.IsSubCategoryIdExist(createProductDto.SubCategoryId);
            if (!subIdExist)
            {
                throw new BadRequestException("SubCategoryId not exist!");
            }

            var codeExist = await _repoProduct.IsCodeExistAsync(createProductDto.Code);
            if (codeExist)
            {
                throw new BadRequestException("Code is exist!");
            }
            var product = createProductDto.ToProductEntity();
            await _repoProduct.AddProductAsync(product);

            var stock = new Stock
            {
                ProductId = product.Id,
                Quantity = createProductDto.Quantity,
                Sold = 0,
            };

            await _repoStock.CreateStockAsync(stock);

            var gallery = createProductDto.Galleries.ToGalleryEntities(product.Id);
            await _repoGallery.CreateGalleryAsync(gallery);

            return product;
        }
    }
}