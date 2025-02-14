using Commerce.BusinessLayer;
using Commerce.EntityLayer.Dtos;
using Commerce.EntityLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        //hem kullanici bilglerini almak icin hem de siparis bilgilerini almak icin servislerimi bagladim
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Product>>> SearchProducts([FromQuery] ProductFilterDto filterDto)
        {
            var products = await _productService.FilterProductsAsync(filterDto);
            return Ok(products);
        }
    }
}
