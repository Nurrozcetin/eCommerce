using Commerce.BusinessLayer;
using Commerce.EntityLayer.Dtos;
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

        //kullanicinin sepetindeki urunleri siparis etmesini saglar.
        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                //siparis verecek kullaniciyi bul
                var userEmail = User.FindFirstValue(ClaimTypes.Email);

                if (userEmail == null)
                    return Unauthorized("Kullanıcı doğrulanamadı.");

                //degiskenle birlikte ilgili fonksiyona yonlendir.
                var order = await _orderService.CreateOrderAsync(userEmail, request.PaymentMethodId, request.AddressId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //kullanicinin kendi siparis ve detaylarini goruntuleyebilmesini saglar.
        [HttpGet("getOrder")]
        public async Task<IActionResult> GetOrder()
        {
            //siparis detaytlarini incelemek isteyen kullaniciyi bul.
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            //degiskenle birlikte ilgili fonksiyona yonlendir.
            var orders = await _orderService.GetAllOrderAsync(userEmail);

            if (orders == null)
                return NotFound("Kullanıcıya ait sipariş bulunamadı");

            return Ok(orders);
        }

        //saticinin hangi musterinin hangi urunlerini siparis ettigini goruntulemeyi saglar.
        [HttpGet("sendOrderDetail")]
        public async Task<IActionResult> GetOrdersBySeller()
        {
            //satici bul.
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            //siparis detaylarini goruntulemek icin fonksiyona yonlendir.
            var orders = await _orderService.GetOrdersBySellerAsync(userEmail);

            if (orders == null)
                return NotFound("Kullanıcıya ait sipariş bulunamadı");

            return Ok(orders);
        }

        //siparisin lojistik uzmanlarinca kargolanmasini saglar.
        [HttpPost("shipOrder/{orderId}")]
        public async Task<IActionResult> ShipOrder(int orderId)
        {
            try
            {
                // Kullanıcı email adresini alıyoruz.
                var userEmail = User.FindFirstValue(ClaimTypes.Email); 

                if (string.IsNullOrEmpty(userEmail))
                    return Unauthorized("Kullanıcı doğrulanamadı!");

                //boolean ifadeyle siparisin kargoya verilip verilmedigini ogrenip sonucu dondurur.
                bool isShipped = await _orderService.ShipOrderAsync(userEmail, orderId);

                if (!isShipped)
                    return BadRequest("Sipariş kargoya verilemedi!");

                return Ok("Sipariş kargoya verildi!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //musterinin siparis ettigi urunu iade edebilmesini saglar.
        [HttpPost("returnOrderItem/{orderItemId}")]
        public async Task<IActionResult> ReturnOrderItem(int orderItemId)
        {
            try
            {
                //iade isteyen kullaniciyi bul
                var userEmail = User.FindFirstValue(ClaimTypes.Email);

                if (string.IsNullOrEmpty(userEmail))
                    return Unauthorized("Kullanıcı doğrulanamadı!");

                //urunun iade edilip edilmedigini ilgil fonksiyona yonlendirerek bul.
                bool isReturned = await _orderService.ReturnOrderItemAsync(userEmail, orderItemId);

                return Ok(isReturned);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
    public class CreateOrderRequest
    {
        public int PaymentMethodId { get; set; }
        public int AddressId { get; set; }
    }
}
