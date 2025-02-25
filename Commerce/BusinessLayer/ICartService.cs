using Commerce.EntityLayer.Models;

namespace Commerce.BusinessLayer
{
    public interface ICartService
    {
        Task AddProductToCartAsync(int userId, int productId, int quantity); //kullanicinin sepetine urun ekleyebilmesini saglar
        Task<List<Product>> GetCartProductsAsync(int userId); //kullanicinin sepetini goruntuleyebilmesini saglar.
        Task DeleteProductCartAsync(int userIdl, int productId); //kullanicinin sepetinden urun kaldirabilmesini saglar.
    }
}
