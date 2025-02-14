using Commerce.BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;

        public CartController(ICartService cartService, IUserService userService)
        {
            _cartService = cartService;
            _userService = userService;
        }

        [HttpPost("addCart")]
        public async Task<IActionResult> AddProductToCart([FromBody] AddCartRequest request)
        {
            try
            {
                //kullaniciyi mailiyle bul
                var userEmail = User.FindFirstValue(ClaimTypes.Email);

                if (userEmail == null)
                    return Unauthorized("Kullanıcı doğrulanamadı.");

                //favouriteService deki fonksiyona yonlendir
                await _cartService.AddProductToCartAsync(userEmail, request.ProductId, request.Quantity);
                return Ok(new { message = "Ürün sepete eklendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        public class AddCartRequest
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        [HttpGet("getCart")]
        public async Task<IActionResult> GetCart()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var cart = await _cartService.GetCartProductsAsync(userEmail);

            if (cart == null)
                return NotFound("Kullanıcıya ait favoriler bulunamadı");

            return Ok(cart);
        }

        [HttpDelete("deleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductRequest request)
        {
            //kullaniciyi bul
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            //secilen urunu service dosyasindaki silme fonksiyonuna yonlendir
            await _cartService.DeleteProductCartAsync(userEmail, request.ProductId);
            return Ok("Ürün kullanıcıya ait sepetten kaldırıldı");
        }
        public class DeleteProductRequest
        {
            public int ProductId { get; set; }
        }
    }
}
