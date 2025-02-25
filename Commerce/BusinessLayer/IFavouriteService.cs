using Commerce.EntityLayer.Models;

namespace Commerce.BusinessLayer
{
    public interface IFavouriteService
    {
        Task AddProductToFavouritesAsync(int userId, int productId); //kullanicinin favorilerine urun ekleyebilmesini saglar.
        Task<List<Product>> GetFavouriteProductsAsync(int userId); //kullanicinin favorilerini goruntuleyebilmesini saglar.
        Task DeleteProductFavouritesAsync(int userId, int productId); //kullanicinin favorilerinden urun kaldirabilmesini saglar.
    }
}
