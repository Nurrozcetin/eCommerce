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
        Task<string> AskQuestionAsync(AskDto askDto, string email); //kullanicinin urun hakkinda soru sormasini saglar.
        Task<List<AskDto>> GetQuestionBySellerAsync(int productId); //saticiya urun ustunden gelen sorularin goruntulenmesini saglar.
        Task<string> AnswerQuestionAsync(AskDto askDto, string email); //saticinin sorulan soruya cevap vermesini saglar.
        Task<string> RateAsync(RateDto rateDto, string userEmail); //kullanicinin belli bir urun hakkinda degerlendirme yapabilmesini saglar.
        Task<List<Rating>> GetRateByProductAsync(int productId); //belli bir urune ait tum degerlendirmeleri listeler.
    }
} 