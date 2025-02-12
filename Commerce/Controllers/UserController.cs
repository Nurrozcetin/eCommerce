using Commerce.BusinessLayer;
using Commerce.EntityLayer;
using Commerce.EntityLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        //kullanici kayit islemi icin controller fonksiyonu 
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (registerDto == null)
                {
                    return BadRequest("Invalid data.");
                }

                var result = await _userService.RegisterAsync(registerDto.Email, registerDto.Password, registerDto.Gender);
                if (result == "Kullanıcı başarıyla oluşturuldu!")
                    return Ok("Kullanıcı başarıyla kaydedildi.");
                return BadRequest("Kullanıcı kaydedilemedi.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); //sunucu kaynakli hata
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            try
            {
                if (loginDto == null)
                {
                    return BadRequest("Invalid data.");
                }

                var user = await _userService.LoginAsync(loginDto.Email, loginDto.Password);

                if (user == null)
                {
                    return Unauthorized("Invalid credentials.");
                }

                var token = _tokenService.GenerateToken(user.Email);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized($"Giriş başarısız: {ex.Message}"); //401 gonderilir
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            return Ok(new { UserEmail = userEmail });
        }
    }
}
