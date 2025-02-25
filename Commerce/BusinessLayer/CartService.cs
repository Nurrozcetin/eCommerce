
using Commerce.DataAccessLayer;
using Commerce.EntityLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Commerce.BusinessLayer
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddProductToCartAsync(int userId, int productId, int quantity)
        {
            // Kullanıcıyı bul
            var user = await _context.Users
                .Include(u => u.Cart)
                    .ThenInclude(c => c.ProductCart)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");

            // Sepete eklenecek ürünü bul
            var product = await _context.Product.FindAsync(productId);
            if (product == null)
                throw new Exception("Ürün bulunamadı");

            // Kullanıcının sepeti var mı kontrol et, yoksa oluştur
            var cart = user.Cart;
            if (cart == null)
            {
                cart = new Cart { UserID = user.Id };
                _context.Cart.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Ürün zaten sepette mi kontrol et
            var existingProduct = cart.ProductCart.FirstOrDefault(pc => pc.ProductID == productId);
            if (existingProduct != null)
                throw new Exception("Ürün zaten sepette mevcut");

            // Sepete ekle
            cart.ProductCart.Add(new ProductCart
            {
                CartID = cart.CartID,
                ProductID = productId,
                Quantity = quantity
            });

            await _context.SaveChangesAsync();
        }
        public async Task<List<Product>> GetCartProductsAsync(int userId)
        {
            //Sepetine erisilecek kullaniciyi bul 
            var user = await _context.Users
             .Include(userInfo => userInfo.Cart)
                 .ThenInclude(cart => cart.ProductCart)
                     .ThenInclude(productCart => productCart.Product)
             .FirstOrDefaultAsync(userInfo => userInfo.Id == userId);

            if (user == null || user.Cart == null || user.Cart.ProductCart == null)
                throw new Exception("Kullanıcıya ait ürün bulunamadı");

            //Secilen kullanicinin sepetindeki urunleri en yeniden en eskiye sirala
            return user.Cart.ProductCart
                .Select(productCart => productCart.Product)
                .OrderByDescending(productCart => productCart.CreatedAt) // En yeniden en eskiye tarihe gore sirala 
                .ToList();
        }
        public async Task DeleteProductCartAsync(int userId, int productId)
        {
            //Sepetine erisilecek kullaniciyi bul 
            var user = await _context.Users
                .Include(userInfo => userInfo.Cart)
                    .ThenInclude(cart => cart.ProductCart)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Cart == null || user.Cart.ProductCart == null)
            {
                throw new Exception("Kullanıcıya ait sepet bulunamadı veya sepet boş.");
            }

            // Kullanıcıya ait sepeti ve ürününü bul
            var productCart = user.Cart.ProductCart
                .FirstOrDefault(pc => pc.ProductID == productId); 

            if (productCart == null)
            {
                throw new Exception("Sepette belirtilen ürün bulunamadı.");
            }

            // Bulunan ürünü sepetten kaldır
            _context.ProductCart.Remove(productCart);

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();
        }
    }
}
