using Commerce.DataAccessLayer;
using Commerce.EntityLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Commerce.BusinessLayer
{
    public class FavouriteService : IFavouriteService
    {
        private readonly AppDbContext _context;

        public FavouriteService(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddProductToFavouritesAsync(int userId, int productId)
        {
            //kullaniciyi bul
            var user = await _context.Users
                .Include(userInfo => userInfo.Favourites)
                    .ThenInclude(fav => fav.ProductFavourites)
                .FirstOrDefaultAsync(userInfo => userInfo.Id == userId);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");

            //favorilere eklenecek urunu sec
            var product = await _context.Product.FindAsync(productId);
            if (product == null)
                throw new Exception("Ürün bulunamadı");

            //kullaniciya ait favori listesi var mi yoksa onun icin bir tane favori listesi olustur
            var favourites = user.Favourites.FirstOrDefault();
            if (favourites == null)
            {
                favourites = new Favourites { UserID = user.Id };
                _context.Favourites.Add(favourites);
                await _context.SaveChangesAsync();
            }

            //eklenecek olan urunun onceden favorilere eklenip eklenmedigini kontrol et eklendiyse cikti ver eklenmediyse listeye secilir urunu ekle
            if (favourites.ProductFavourites.Any(productFavourite => productFavourite.ProductID == productId))
                throw new Exception("Ürün zaten favorilere eklenmiş");

            favourites.ProductFavourites.Add(new ProductFavourites
            {
                FavouritesID = favourites.FavouritesID,
                ProductID = productId
            });

            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetFavouriteProductsAsync(int userId)
        {
            //Favorilerine erisilecek kullaniciyi bul 
            var user = await _context.Users
             .Include(userInfo => userInfo.Favourites)
                 .ThenInclude(fav => fav.ProductFavourites)
                     .ThenInclude(productfav => productfav.Product)
             .FirstOrDefaultAsync(userInfo => userInfo.Id == userId);

            if (user == null || user.Favourites == null)
                throw new Exception("Favori ürün bulunamadı");

            //Secilen kullanicinin favorilerindeki urunleri en yeniden en eskiye sirala
            return user.Favourites
                .SelectMany(fav => fav.ProductFavourites.Select(productfav => productfav.Product))
                .OrderByDescending(productfav => productfav.CreatedAt) // En yeniden en eskiye tarihe gore sirala 
                .ToList();
        }

        public async Task DeleteProductFavouritesAsync(int userId, int productId)
        {
            //kullaniciyi mail yoluyla bul
            var user = await _context.Users
                .Include(u => u.Favourites)
                    .ThenInclude(f => f.ProductFavourites)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");

            //kullaniciya ait favorileri bul
            var favourites = user.Favourites.FirstOrDefault();
            if (favourites == null)
                throw new Exception("Favori listesi bulunamadı");

            //kullaniciya ait favori listesinde secilen urunu ara bul
            var productFavourite = favourites.ProductFavourites.FirstOrDefault(pf => pf.ProductID == productId);
            if (productFavourite == null)
                throw new Exception("Ürün favorilerde bulunamadı");

            //bulunan urunu favorilerden kaldir
            favourites.ProductFavourites.Remove(productFavourite);
            await _context.SaveChangesAsync();
        }
    }
}
