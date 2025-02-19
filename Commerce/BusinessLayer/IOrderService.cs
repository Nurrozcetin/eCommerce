using Commerce.EntityLayer.Dtos;
using Commerce.EntityLayer.Models;

namespace Commerce.BusinessLayer
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string email, int paymentMethodId, int addressId); //kullanicinin sepetindeki urunleri siparis etmesini saglar.
        Task<List<Order>> GetAllOrderAsync(string email); //kullanicinin kendi siparis ve detaylarini goruntuleyebilmesini saglar.
        Task<List<OrderDetailDto>> GetOrdersBySellerAsync(string email); //saticinin hangi musterinin hangi urunlerini siparis ettigini goruntulemeyi saglar.
        Task<bool> ShipOrderAsync(string email, int orderId); //siparisin lojistik uzmanlarinca kargolanmasini saglar.
        Task<bool> ReturnOrderItemAsync(string email, int orderItemId); //musterinin siparis ettigi urunu iade edebilmesini saglar.
    }
}
 