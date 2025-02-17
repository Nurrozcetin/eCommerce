using Commerce.EntityLayer.Dtos;
using Commerce.EntityLayer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Commerce.BusinessLayer
{
    public interface IProductService
    {
        Task<List<Product>> FilterProductsAsync(ProductFilterDto filterDto); //kullanıcıların belli filtrelere göre arama yapmasını sağlar.
        Task<List<ProductDto>> ListAllProductAsync(); //tüm ürünlerin belli nitelikleri de baz alınarak listelenmesini sağlar.
        Task<List<ProductDto>> ListProductAsync(int productId); //seçili ürüne ait belli niteliklerin görüntülenmesini sağlar.
        Task<List<ProductDto>> ListProductByCategoryAsync(int categoryId); //belli kategoriye ait tüm ürünlerin listelenmesini sağlar.
    }
}
