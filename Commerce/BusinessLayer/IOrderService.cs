using Commerce.EntityLayer.Models;

namespace Commerce.BusinessLayer
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string email, int paymentMethodId, int addressId);
        Task<List<Order>> GetAllOrderAsync(string email);
    }
}
 