using Commerce.EntityLayer.Dtos;
using Commerce.EntityLayer.Models;

namespace Commerce.BusinessLayer
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int userId, int paymentMethodId, int addressId); //kullanicinin sepetindeki urunleri siparis etmesini saglar.
        Task<List<Order>> GetAllOrderAsync(int userId); //kullanicinin kendi siparis ve detaylarini goruntuleyebilmesini saglar.
        Task<List<OrderDetailDto>> GetOrdersBySellerAsync(int userId); //saticinin hangi musterinin hangi urunlerini siparis ettigini goruntulemeyi saglar.
        Task<bool> ShipOrderAsync(int userId, int orderId); //siparisin lojistik uzmanlarinca kargolanmasini saglar.
        Task<bool> ReturnOrderItemAsync(int userId, int orderItemId); //musterinin siparis ettigi urunu iade edebilmesini saglar.
    }
}
 