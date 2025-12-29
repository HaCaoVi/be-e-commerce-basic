using e_commerce_basic.Helpers;
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
        private readonly ApplicationDBContext _dbContext;
        private readonly IProductRepository _repoProduct;
        private readonly ISubCategoryRepository _repoSubCategory;
        private readonly IStockRepository _repoStock;
        private readonly IGalleryRepository _repoGallery;
        public ProductService(ApplicationDBContext dbContext, IProductRepository repoProduct, ISubCategoryRepository repoSubCategory, IStockRepository repoStock, IGalleryRepository repoGallery)
        {
            _dbContext = dbContext;
            _repoProduct = repoProduct;
            _repoSubCategory = repoSubCategory;
            _repoStock = repoStock;
            _repoGallery = repoGallery;
        }

        public async Task<ProductDto> HandleAddProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
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

                if (createProductDto.TypeDiscount == Types.EDiscount.Percent && createProductDto.Discount > 100)
                {
                    throw new BadRequestException("Discount percent must be between 0 and 100");
                }

                if (createProductDto.Price < createProductDto.Discount)
                {
                    throw new BadRequestException("Price must be greater than discount");
                }

                var product = createProductDto.ToProductEntity();
                await _repoProduct.AddAsync(product);

                var stock = new Stock
                {
                    ProductId = product.Id,
                    Quantity = createProductDto.Quantity,
                    Sold = 0,
                };

                await _repoStock.CreateStockAsync(stock);

                var gallery = createProductDto.Galleries.ToGalleryEntities(product.Id);
                await _repoGallery.CreateGalleryAsync(gallery);

                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return product.ToProductDto();
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<ProductDto> HandleGetByIdProductAsync(int id)
        {
            var product = await _repoProduct.GetByIdAsync(id) ?? throw new KeyNotFoundException("Product not found");
            return product.ToProductDto();
        }

        public async Task<PagedResult<ProductDto>> HandleGetListProductAsync(QueryObject query, CancellationToken cancellationToken)
        {
            var pagedProducts = await _repoProduct.GetAllAsync(query, cancellationToken);
            var dtoPaged = new PagedResult<ProductDto>
            {
                Meta = pagedProducts.Meta,
                Result = pagedProducts.Result.ConvertAll(p => p.ToProductDto())
            };
            return dtoPaged;
        }

    }
}