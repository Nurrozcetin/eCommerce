
using Commerce.DataAccessLayer;
using Commerce.EntityLayer.Dtos;
using Commerce.EntityLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Commerce.BusinessLayer
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context; 

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        //kullanicinin sepetindeki urunleri siparis etmesini saglar.
        public async Task<Order> CreateOrderAsync(string Email, int paymentMethodId, int addressId)
        {
            //var olan kullaniciyi getiriyoruz
            var user = _context.Users.Include(u => u.Cart)
                             .ThenInclude(c => c.ProductCart)
                             .ThenInclude(pc => pc.Product)
                             //.ThenInclude(pc => pc.Seller)
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
                TotalPrice = cartItems.Sum(ci => ci.Quantity * ci.Product.Price),
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
                Price = ci.Product.Price
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

        //kullanicinin kendi siparis ve detaylarini goruntuleyebilmesini saglar.
        public async Task<List<Order>> GetAllOrderAsync(string email)
        {
            //kullaniciyi bul 
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");

            return await _context.Order
                .Where(o => o.UserID == user.Id) // Kullaniciya ait siparis bul
                .OrderByDescending(o => o.CreatedAt) // Tarihe gore sirala 
                .Include(o => o.OrderItems) // Siparis icerisindeki urunleri cek 
                    .ThenInclude(oi => oi.Product) 
                .ToListAsync(); //listele 
        }

        //saticinin hangi musterinin hangi urunlerini siparis ettigini goruntulemeyi saglar.
        public async Task<List<OrderDetailDto>> GetOrdersBySellerAsync(string email)
        {
            //satıcı bulunur.
            var seller = await _context.Users
               .Where(u => u.Email == email)
               .Select(u => u.Id)
               .FirstOrDefaultAsync();

            //hangi ürünün sipariş edildiğini bul.
            var orders = await _context.OrderItem
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .Where(oi => oi.Product.SellerId == seller)
                .Select(oi => new OrderDetailDto
                {
                    OrderId = oi.OrderID,
                    OrderDate = oi.Order.CreatedAt,
                    CustomerEmail = oi.Order.User.Email,
                    ProductName = oi.Product.ProductName,
                    Quantity = oi.Quantity,
                    TotalPrice = oi.Quantity * oi.Price
                })
                .ToListAsync();

            return orders;
        }

        //siparisin lojistik uzmanlarinca kargolanmasini saglar.
        public async Task<bool> ShipOrderAsync(string email, int orderId)
        {
            var user = await _context.Users
                                    .Include(u => u.Role) // Kullanıcının rollerini almak için
                                    .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");

            // Kullanıcının 'Logistics Specialist' rolüne sahip olup olmadığını kontrol ediyoruz
            if (user.Role.RoleName != "Logistics Specialist")
                throw new Exception("Bu işlemi yapma yetkiniz yok!");

            var order = await _context.Order.FirstOrDefaultAsync(o => o.OrderID == orderId);

            if (order == null)
                throw new Exception("Sipariş bulunamadı!");

            // Siparişin durumunu "Shipped" (Kargoya verildi) olarak değiştiriyoruz
            order.StatusID = 2;

            await _context.SaveChangesAsync();
            return true; // Güncelleme başarılı
        }

        //musterinin siparis ettigi urunu iade edebilmesini saglar.
        public async Task<bool> ReturnOrderItemAsync(string email, int orderItemId)
        {
            //urunu iade edecek kullaniciyi bul.
            var user = await _context.Users
        .       FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");

            //iade edilecek urunu bul.
            var orderItem = await _context.OrderItem
                 .Include(oi => oi.Order)
                 .Include(oi => oi.Status) // Status bilgisini de dahil ediyoruz
                 .FirstOrDefaultAsync(oi => oi.ItemID == orderItemId);

            if (orderItem == null)
                throw new Exception("Sipariş kalemi bulunamadı!");

            if (orderItem.StatusID == 4)
                throw new Exception("Bu ürün zaten iade edildi!");

            // Ürünün iade ediyoruz
            orderItem.StatusID = 4;

            await _context.SaveChangesAsync();
            return true; // İade başarılı
        }
    }
} 
