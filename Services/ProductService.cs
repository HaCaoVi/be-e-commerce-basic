using e_commerce_basic.Database;
using e_commerce_basic.Dtos.Product;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Mappings;
using e_commerce_basic.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_basic.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDBContext _context;
        public ProductService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProductAsync(CreateProductDto createProductDto)
        {
            var subIdExist = await _context.SubCategories.AnyAsync(s => s.Id == createProductDto.SubCategoryId);
            if (!subIdExist)
            {
                throw new InvalidOperationException("SubCategoryId not exist!");
            }

            var codeExist = await _context.Products.AnyAsync(p => p.Code == createProductDto.Code);
            if (codeExist)
            {
                throw new InvalidOperationException("Code is exist!");
            }

            var product = createProductDto.ToProductEntity();
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            var stock = new Stock
            {
                ProductId = product.Id,
                Quantity = createProductDto.Quantity
            };

            await _context.Stocks.AddAsync(stock);

            var gallery = createProductDto.Galleries.ToGalleryEntities(product.Id);
            await _context.Galleries.AddRangeAsync(gallery);
            await _context.SaveChangesAsync();
            var result = await _context.Products
                .Include(p => p.Stock)
                .Include(p => p.SubCategory)
                .Include(p => p.Galleries)
                .FirstAsync(p => p.Id == product.Id);

            return result;
        }
    }
}