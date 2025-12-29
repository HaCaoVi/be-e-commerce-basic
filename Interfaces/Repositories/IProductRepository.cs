using e_commerce_basic.Helpers;
using e_commerce_basic.Models;

namespace e_commerce_basic.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        Task<bool> IsCodeExistAsync(string code);
        Task<PagedResult<Product>> GetAllAsync(QueryObject query, CancellationToken cancellationToken);
        Task<Product?> GetByIdAsync(int id);
    }
}