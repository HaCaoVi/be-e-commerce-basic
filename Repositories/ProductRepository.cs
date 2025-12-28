using e_commerce_basic.Database;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_basic.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await _context.AddAsync(product);
            return product;
        }

        public async Task<bool> IsCodeExistAsync(string code)
        {
            var codeExist = await _context.Products.AnyAsync(p => p.Code == code);
            return codeExist;
        }
    }
}