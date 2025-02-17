using Commerce.BusinessLayer;
using Commerce.EntityLayer.Dtos;
using Commerce.EntityLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        //tüm ürünleri çek.
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> ListAllProductAsync()
        {
            //ilgili fonksiyona yönlendir, listeyi döndür.
            var productDtoList = await _productService.ListAllProductAsync();
            return Ok(productDtoList);
        }

        //girilen ürün id sine ait olan ürünü ve ürüne ait nitelikleri çek.
        [HttpGet("{productId}")]
        public async Task<IActionResult> ListProduct(int productId)
        {
            //ilgili fonksiyona yönlendir products değişkenine ata, products null ise o ürün id sine ait bir ürün yoktur, products null değilse ürünü döndür.
            var products = await _productService.ListProductAsync(productId);
            if (products == null)
            {
                return NotFound("Seçili ürün bulunamadı");
            }
            return Ok(products);
        }

        //girilen kategori id sine ait olan ürünleri ve ürünlere ait nitelikleri çek.
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> ListProductByCategory(int categoryId)
        {
            //ilgili fonksiyona yönlendir products değişkenine ata, products null ise o kategori id sine ait bir ürün yoktur, products null değilse kategoriye ait ürünleri döndür.
            var products = await _productService.ListProductByCategoryAsync(categoryId);
            if(products == null)
            {
                return NotFound("Bu kategoriye uygun ürün bulunamadı");
            }
            return Ok(products);
        }
    }
}
