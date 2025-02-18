using Commerce.BusinessLayer;
using Commerce.EntityLayer.Dtos;
using Commerce.EntityLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            if (products == null)
            {
                return NotFound("Bu kategoriye uygun ürün bulunamadı");
            }
            return Ok(products);
        }

        //kullanicinin urun hakkinda soru sormasini saglar.
        [Authorize]
        [HttpPost("ask")]
        public async Task<IActionResult> AskQuestion([FromBody] AskDto askDto)
        {
            //soru soracak kullaniciyi bul.
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            //result degiskeni adi altinda ilgili fonksiyona yonlendir ve sonucu dondur.
            var result = await _productService.AskQuestionAsync(askDto, userEmail);
            if (result == null)
                return NotFound("Ürün veya kullanıcı bulunamadı!");

            return Ok(result);
        }

        //saticiya urun ustunden gelen sorularin goruntulenmesini saglar.
        [HttpGet("question/{productId}")]
        public async Task<IActionResult> GetQuestionBySeller(int productId)
        {
            //ilgili fonksiyona yönlendir questions değişkenine ata, questions null ise o urun id sine ait bir soru yoktur, questions null değilse urune ait sorulari döndür.
            var questions = await _productService.GetQuestionBySellerAsync(productId);
            if (questions == null)
            {
                return NotFound("Bu ürüne ait soru bulunamadı");
            }
            return Ok(questions);
        }

        //saticinin sorulan soruya cevap vermesini saglar.
        [Authorize]
        [HttpPost("answer")]
        public async Task<IActionResult> AnswerQuestion([FromBody] AskDto askDto)
        {
            //saticiyi bul.
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            //result degiskeni adi altinda ilgili fonksiyona yonlendir ve sonucu dondur.
            var result = await _productService.AnswerQuestionAsync(askDto, userEmail);
            if (result == null)
                return NotFound("Soru doğru bir şekilde cevaplanamadı!");

            return Ok(result);
        }

        //kullanicinin belli bir urun hakkinda degerlendirme yapabilmesini saglar.
        [Authorize]
        [HttpPost("rate")]
        public async Task<IActionResult> AddRating([FromBody] RateDto rateDto)
        {
            //email ile degerlendirme yapacak kullaniciyi al.
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
                return Unauthorized("Kullanıcı doğrulaması yapılamadı.");

            //result degiskeniyle fonksiyona yonlendir sonucu don.
            var result = await _productService.RateAsync(rateDto, userEmail);
            return Ok(result);
        }

        //belli bir urune ait tum degerlendirmeleri listeler.
        [HttpGet("rate/{productId}")]
        public async Task<IActionResult> GetRateByProduct(int productId)
        {
            //ratings degiskeniyle parametre olarak alinan productId yi de kullanarak ilgili fonksiyona yonlendir.
            var ratings = await _productService.GetRateByProductAsync(productId);
            if (ratings.Count == 0)
                return NotFound("Bu ürün için henüz değerlendirme yapılmadı.");

            return Ok(ratings);
        }
    }
}
