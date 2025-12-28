using e_commerce_basic.Models;

namespace e_commerce_basic.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
        Task<bool> IsCodeExistAsync(string code);
    }
}