using e_commerce_basic.Helpers;
using e_commerce_basic.Models;

namespace e_commerce_basic.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
        Task<bool> IsCodeExistAsync(string code);
        Task<PagedResult<Product>> GetPagedProductsAsync(QueryObject query, CancellationToken cancellationToken);
    }
}