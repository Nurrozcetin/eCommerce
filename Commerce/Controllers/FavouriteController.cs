using Commerce.BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;
        private readonly IUserService _userService;

        public FavouriteController(IFavouriteService favouriteService, IUserService userService)
        {
            _favouriteService = favouriteService;
            _userService = userService;
        }

        [HttpPost("addFavourite")]
        public async Task<IActionResult> AddProductToFavourites([FromBody] AddFavouriteRequest request)
        {
            try
            {
                //kullaniciyi mailiyle bul
                var userEmail = User.FindFirstValue(ClaimTypes.Email);

                if (userEmail == null)
                    return Unauthorized("Kullanıcı doğrulanamadı.");

                //favouriteService deki fonksiyona yonlendir
                await _favouriteService.AddProductToFavouritesAsync(userEmail, request.ProductId);
                return Ok(new { message = "Ürün favorilere eklendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        public class AddFavouriteRequest
        {
            public int ProductId { get; set; }
        }

        [HttpGet("getFavourite")]
        public async Task<IActionResult> GetFavourite()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var favourite = await _favouriteService.GetFavouriteProductsAsync(userEmail);

            if (favourite == null)
                return NotFound("Kullanıcıya ait favoriler bulunamadı");

            return Ok(favourite);
        }

        [HttpDelete("deleteProduct")]
        public async Task<IActionResult> DeleteProductFavourites([FromBody] DeleteProductRequest request)
        {
            //kullaniciyi bul
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            //secilen urunu service dosyasindaki silme fonksiyonuna yonlendir
            await _favouriteService.DeleteProductFavouritesAsync(userEmail, request.ProductId);
            return Ok("Ürün favorilerden kaldırıldı");
        }
        public class DeleteProductRequest
        {
            public int ProductId { get; set; }
        }
    }
}
