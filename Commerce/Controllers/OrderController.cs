using Commerce.BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        //hem kullanici bilglerini almak icin hem de siparis bilgilerini almak icin servislerimi bagladim
        public OrderController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);

                if (userEmail == null)
                    return Unauthorized("Kullanıcı doğrulanamadı.");

                var order = await _orderService.CreateOrderAsync(userEmail, request.PaymentMethodId, request.AddressId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("getOrder")]
        public async Task<IActionResult> GetOrder()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetAllOrderAsync(userEmail);

            if (orders == null)
                return NotFound("Kullanıcıya ait sipariş bulunamadı");

            return Ok(orders);
        }
    }
    public class CreateOrderRequest
    {
        public int PaymentMethodId { get; set; }
        public int AddressId { get; set; }
    }
}
