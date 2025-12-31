using e_commerce_basic.Dtos.Product;
using e_commerce_basic.Helpers;
using e_commerce_basic.Models;

namespace e_commerce_basic.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        Task<bool> IsCodeExistAsync(string code, int? excludeProductId);
        Task<PagedResult<Product>> GetAllAsync(QueryObject query, CancellationToken cancellationToken);
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> ProductById(int id);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    }
}