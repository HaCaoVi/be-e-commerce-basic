using e_commerce_basic.Helpers;
using e_commerce_basic.Common;
using e_commerce_basic.Database;
using e_commerce_basic.Dtos.Product;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Mappings;
using e_commerce_basic.Models;
using e_commerce_basic.Types;

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
                if (createProductDto.Price < createProductDto.Discount)
                {
                    throw new BadRequestException("Price must be greater than discount");
                }

                var subIdExist = await _repoSubCategory.IsSubCategoryIdExist(createProductDto.SubCategoryId);
                if (!subIdExist)
                {
                    throw new BadRequestException("SubCategoryId not exist!");
                }

                var codeExist = await _repoProduct.IsCodeExistAsync(createProductDto.Code, null);
                if (codeExist)
                {
                    throw new BadRequestException("Code is exist!");
                }

                var product = createProductDto.ToProductEntity();
                product.Stock = new Stock
                {
                    Quantity = createProductDto.Quantity,
                    Sold = 0,
                };

                product.Galleries = createProductDto.Galleries.ToGalleryEntities();

                await _repoProduct.AddAsync(product);

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

        public async Task<ProductDto> HandleUpdateProductAsync(int id, UpdateProductDto updateProductDto, CancellationToken cancellationToken)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                if (updateProductDto.Price < updateProductDto.Discount)
                {
                    throw new BadRequestException("Price must be greater than discount");
                }

                var product = await _repoProduct.ProductById(id) ?? throw new KeyNotFoundException("Product not found");
                var subIdExist = await _repoSubCategory.IsSubCategoryIdExist(updateProductDto.SubCategoryId);
                if (!subIdExist)
                {
                    throw new BadRequestException("SubCategoryId not exist!");
                }
                var isCodeExist = await _repoProduct.IsCodeExistAsync(updateProductDto.Code, id);
                if (isCodeExist)
                {
                    throw new BadRequestException("Code is exist");
                }

                product.Code = updateProductDto.Code;
                product.Name = updateProductDto.Name;
                product.Description = updateProductDto.Description;
                product.Price = updateProductDto.Price;
                product.Discount = updateProductDto.Discount;
                product.TypeDiscount = updateProductDto.TypeDiscount;
                product.IsActivated = updateProductDto.IsActivated;
                product.SubCategoryId = updateProductDto.SubCategoryId;
                product.UpdatedAt = DateTime.UtcNow;

                await _repoStock.UpdateAsync(product.Id, updateProductDto.Quantity);

                await _repoGallery.UpdateAsync(product.Id, updateProductDto.Galleries.ToGalleryEntities());

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
    }
}