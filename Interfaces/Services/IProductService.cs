using e_commerce_basic.Helpers;
using e_commerce_basic.Dtos.Product;

namespace e_commerce_basic.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> HandleAddProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken);
        Task<PagedResult<ProductDto>> HandleGetListProductAsync(QueryObject query, CancellationToken cancellationToken);
        Task<ProductDto> HandleGetByIdProductAsync(int id);
    }
}