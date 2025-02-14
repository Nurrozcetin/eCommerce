using Commerce.EntityLayer.Dtos;
using Commerce.EntityLayer.Models;

namespace Commerce.BusinessLayer
{
    public interface IProductService
    {
        Task<List<Product>> FilterProductsAsync(ProductFilterDto filterDto);
    }
}
