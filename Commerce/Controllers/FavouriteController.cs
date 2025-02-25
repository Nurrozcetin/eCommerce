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
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(userId, out int parsedUserId))
                {
                    return Unauthorized("Kullanıcı doğrulanamadı.");
                }

                //favouriteService deki fonksiyona yonlendir
                await _favouriteService.AddProductToFavouritesAsync(parsedUserId, request.ProductId);
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out int parsedUserId))
            {
                return Unauthorized("Kullanıcı doğrulanamadı.");
            }

            var favourite = await _favouriteService.GetFavouriteProductsAsync(parsedUserId);

            if (favourite == null)
                return NotFound("Kullanıcıya ait favoriler bulunamadı");

            return Ok(favourite);
        }

        [HttpDelete("deleteProduct")]
        public async Task<IActionResult> DeleteProductFavourites([FromBody] DeleteProductRequest request)
        {
            //kullaniciyi bul
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out int parsedUserId))
            {
                return Unauthorized("Kullanıcı doğrulanamadı.");
            }

            //secilen urunu service dosyasindaki silme fonksiyonuna yonlendir
            await _favouriteService.DeleteProductFavouritesAsync(parsedUserId, request.ProductId);
            return Ok("Ürün favorilerden kaldırıldı");
        }
        public class DeleteProductRequest
        {
            public int ProductId { get; set; }
        }
    }
}
