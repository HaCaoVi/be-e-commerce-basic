using e_commerce_basic.Dtos.Product;
using e_commerce_basic.Models;

namespace e_commerce_basic.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
        Task<bool> IsCodeExistAsync(string code);
    }
}