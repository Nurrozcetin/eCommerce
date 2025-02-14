using Commerce.EntityLayer.Models;

namespace Commerce.BusinessLayer
{
    public interface IFavouriteService
    {
        Task AddProductToFavouritesAsync(string email, int productId); //kullanicinin favorilerine urun ekleyebilmesini saglar.
        Task<List<Product>> GetFavouriteProductsAsync(string email); //kullanicinin favorilerini goruntuleyebilmesini saglar.
        Task DeleteProductFavouritesAsync(string email, int productId); //kullanicinin favorilerinden urun kaldirabilmesini saglar.
    }
}
