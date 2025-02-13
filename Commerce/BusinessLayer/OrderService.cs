
using Commerce.DataAccessLayer;
using Commerce.EntityLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Commerce.BusinessLayer
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context; 

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(string Email, int paymentMethodId, int addressId)
        {
            //var olan kullaniciyi getiriyoruz
            var user = _context.Users.Include(u => u.Cart)
                             .ThenInclude(c => c.ProductCart)
                             .ThenInclude(pc => pc.Product)
                             .FirstOrDefault(u => u.Email == Email);

            //kullanicinin sepetinde urun var mi onu kontrol ediyoruz
            if (user == null || user.Cart == null || user.Cart.ProductCart.Count == 0)
                throw new Exception("Sepetinizde ürün bulunmamaktadır!");

            //sepetteki urunleri cekiyoruz
            var cartItems = user.Cart.ProductCart;

            //siparis olusturuyoruz
            var order = new Order
            {
                UserID = user.Id,
                CreatedAt = DateTime.UtcNow,
                TotalPrice = cartItems.Sum(ci => ci.Quantity * decimal.Parse(ci.Product.Price)),
                StatusID = 1
            };

            _context.Order.Add(order);
            _context.SaveChanges();

            //sepetten alinan urunleri siparis kismina ekliyoruz
            var orderItems = cartItems.Select(ci => new OrderItem
            {
                OrderID = order.OrderID,
                ProductID = ci.ProductID,
                Quantity = ci.Quantity,
                Price = decimal.Parse(ci.Product.Price)
            }).ToList();

            _context.OrderItem.AddRange(orderItems);

            //odeme kismi icin bilgileri ekliyoruz
            var payment = new Payment
            {
                Amount = order.TotalPrice,
                OrderID = order.OrderID,
                PaymentMethod = _context.PaymentMethod.FirstOrDefault(pm => pm.PaymentMethodID == paymentMethodId),
                StatusID = 1 
            };

            _context.Payment.Add(payment);

            //urunler siparis kismina gectigi icin sepetten kaldir
            _context.ProductCart.RemoveRange(cartItems);
            await _context.SaveChangesAsync();


            return order;
        }
    }
}
