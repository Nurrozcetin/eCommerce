using Commerce.EntityLayer.Models;

namespace Commerce.BusinessLayer
{
    public interface ICartService
    {
        Task AddProductToCartAsync(string email, int productId, int quantity); //kullanicinin sepetine urun ekleyebilmesini saglar
        Task<List<Product>> GetCartProductsAsync(string email); //kullanicinin sepetini goruntuleyebilmesini saglar.
        Task DeleteProductCartAsync(string email, int productId); //kullanicinin sepetinden urun kaldirabilmesini saglar.
    }
}
