using e_commerce_basic.Dtos.Product;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Mappings;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            var product = await _productService.HandleAddProductAsync(createProductDto);
            return Ok(product.ToProductDto());
        }
    }
}