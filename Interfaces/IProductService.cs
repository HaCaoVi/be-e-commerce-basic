using e_commerce_basic.Dtos.Product;
using e_commerce_basic.Models;

namespace e_commerce_basic.Interfaces
{
    public interface IProductService
    {
        Task<Product> HandleAddProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken);
    }
}