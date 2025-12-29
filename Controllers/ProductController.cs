using e_commerce_basic.Helpers;
using e_commerce_basic.Dtos.Product;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Mappings;
using Microsoft.AspNetCore.Mvc;
using e_commerce_basic.Common;
using Microsoft.AspNetCore.Authorization;

namespace e_commerce_basic.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto, CancellationToken cancellationToken)
        {
            var product = await _productService.HandleAddProductAsync(createProductDto, cancellationToken);
            return Ok(ApiResponse<ProductDto>.Ok(product));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductDto>>> GetList([FromQuery] QueryObject query, CancellationToken cancellationToken)
        {
            var products = await _productService.HandleGetListProduct(query, cancellationToken);
            return Ok(ApiResponse<PagedResult<ProductDto>>.Ok(products));
        }
    }
}