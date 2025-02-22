using Commerce.DataAccessLayer;
using Commerce.EntityLayer.Dtos;
using Commerce.EntityLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Commerce.BusinessLayer
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context; 

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        //kullanıcıların belli filtrelere göre arama yapmasını sağlar.
        public async Task<List<Product>> FilterProductsAsync(ProductFilterDto filterDto)
        {
            var query = _context.Product.AsQueryable();

            if (!string.IsNullOrEmpty(filterDto.ProductName))
            {
                query = query.Where(product => product.ProductName.Contains(filterDto.ProductName));
            }

            if (filterDto.CategoryId.HasValue)
            {
                query = query.Where(product => product.ProductCategory.Any(category => category.CategoryId == filterDto.CategoryId));
            }

            if (filterDto.ColorId.HasValue)
            {
                query = query.Where(product => product.ProductColors.Any(color => color.ColorId == filterDto.ColorId));
            }

            if (filterDto.SizeId.HasValue)
            {
                query = query.Where(product => product.ProductSizes.Any(size => size.SizeId == filterDto.SizeId));
            }

            if (filterDto.MinPrice.HasValue)
            {
                query = query.Where(product => product.Price >= filterDto.MinPrice);
            }

            if (filterDto.MaxPrice.HasValue)
            {
                query = query.Where(product => product.Price <= filterDto.MaxPrice);
            }

            if (filterDto.MinStock.HasValue)
            {
                query = query.Where(product => product.ProductCategory.Any(stock => stock.Stock >= filterDto.MinStock));
            }

            if (filterDto.MaxStock.HasValue)
            {
                query = query.Where(product => product.ProductCategory.Any(stock => stock.Stock <= filterDto.MaxStock));
            }

            return await query.ToListAsync();
        }

        //tüm ürünlerin belli nitelikleri de baz alınarak listelenmesini sağlar.
        public async Task<List<ProductDto>> ListAllProductAsync()
        {
            //tüm ürünleri çek.
            var products = await _context.Product 
                .Include(product => product.Seller)
                .Include(p => p.Ratings)
                .ToListAsync();

            //ürünlerin id sini, ismini, ürüne ait resimleri, fiyatı, stok sayısını, tarihini ve satıcısını çek.
            var productDtoList = products.Select(product => new ProductDto
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                ProductImage = product.ProductImage,
                Price = product.Price,
                Stock = product.Stock, 
                CreatedAt = product.CreatedAt, 
                SellerName = product.Seller != null ? product.Seller.Name : "Bilinmiyor",
                AverageRating = product.Ratings.Any() ? product.Ratings.Average(r => r.Score) : 0,
                RatingsCount = product.Ratings.Count
            }).ToList();

            //çekilen ürünleri listele.
            return productDtoList;
        }

        //seçili ürüne ait belli niteliklerin görüntülenmesini sağlar.
        public async Task<List<ProductDto>> ListProductAsync(int productId)
        {
            //parametre olarak alınan ürün id sine ait olan ürünü çek.
            var product = await _context.Product 
                .Where(p => p.ProductID == productId)
                .Include(p => p.Seller)
                .FirstOrDefaultAsync();

            // ürün yoksa boş liste döndür.
            if (product == null)
                return new List<ProductDto>();

            //girilen id ye ait ürünün id sini, ismini, ürüne ait resimleri, fiyatı, stok sayısını, tarihini ve satıcısını çek.
            var productDtoList = new List<ProductDto>
            {
                new ProductDto
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    ProductImage = product.ProductImage,
                    Price = product.Price,
                    Stock = product.Stock,
                    CreatedAt = product.CreatedAt,
                    SellerName = product.Seller != null ? product.Seller.Name : "Bilinmiyor"
                }
            };

            //çekilen ürünü listele.
            return productDtoList;
        }

        //belli kategoriye ait tüm ürünlerin listelenmesini sağlar.
        public async Task<List<ProductDto>> ListProductByCategoryAsync(int categoryId)
        {
            //parametre olarak alınan kategori id sine uygun kategoriye sahip ürünlere ait ürün id sini, ismini, resimleri, fiyat bilgisini ve stok sayısını çek.
            return await _context.ProductCategories
               .Where(pc => pc.CategoryId == categoryId)
               .Include(pc => pc.Product)
               .Select(pc => new ProductDto
               {
                   ProductID = pc.Product.ProductID,
                   ProductName = pc.Product.ProductName,
                   ProductImage = pc.Product.ProductImage,
                   Price = pc.Product.Price,
                   Stock = pc.Product.Stock
               })
               .ToListAsync();
        }

        //kullanicinin ilgili urunle ilgii soru sorabilmesini saglar.
        public async Task<string> AskQuestionAsync(AskDto askDto, string userEmail)
        {
            //girilen urun id sine ait urunu bul.
            var product = await _context.Product.FindAsync(askDto.ProductId);
            if(product == null)
                return "Ürün bulunamadı";

            //soru soracak kullaniciyi bul.
            var users = await _context.Users.FirstOrDefaultAsync(user => user.Email == userEmail);
            if (users == null)
                return "Kullanıcı bulunamadı";

            //sonra girilen soru metnini Question tablosuna diger bilgilerle ekle.
            var question = new Question
            {
                UserID = users.Id,
                ProductID = askDto.ProductId,
                Questions = askDto.Content,
                Answer = "",
                CreatedAt = DateTime.UtcNow,
            };
            _context.Question.Add(question);
            await _context.SaveChangesAsync();
            return "Sorunuz başarıyla satıcıya iletildi";
        }

        //saticinin ilgili urune ait tum sorulari verilmisse cevaplari goruntuleyebilmesini saglar.
        public async Task<List<AskDto>> GetQuestionBySellerAsync(int productId)
        {
            //parametre olarak alinan urun id sine uygun sekilde sorulan sorular ve cevaplari liste halinde goruntulenir.
            var questions = await _context.Question
                .Where(question => question.Product.ProductID == productId)
                .Include(question => question.Product)
                .Select(question => new AskDto
                {
                    UserId = question.UserID,
                    QuestionId = question.QuestionID,
                    ProductId = question.ProductID,
                    Content = question.Questions,
                    Answers = question.Answer,
                    CreatedAt = question.CreatedAt
                }).ToListAsync();

            return questions;
        }

        //saticinin ilgili urune ait cevap verebilmesini saglar.
        public async Task<string> AnswerQuestionAsync(AskDto askDto, string userEmail)
        {
            //saticinin sorulan sorulara cevap verebilmesi icin kullaniciyi bul.
            var users = await _context.Users.FirstOrDefaultAsync(user => user.Email == userEmail);
            if (users == null)
                return "Kullanıcı bulunamadı.";

            //cevaplanacak soruyu bul.
            var question = await _context.Question
                .FirstOrDefaultAsync(q => q.QuestionID == askDto.QuestionId && q.Product.SellerId == users.Id);

            if (question == null)
                return "Bu ürüne ait soru bulunamadı ya da bu soruya cevap verme yetkiniz yok.";

            // Cevabı güncelle
            question.Answer = askDto.Answers;
            question.CreatedAt = DateTime.UtcNow;

            _context.Question.Update(question);
            await _context.SaveChangesAsync();

            return "Cevabınız başarıyla kaydedildi.";
        }

        //kullanicinin belli bir urun hakkinda degerlendirme yapabilmesini saglar.
        public async Task<string> RateAsync(RateDto rateDto, string userEmail)
        {
            //email ile degerlendirilecek kullaniciyi bul.
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
                return "Kullanıcı bulunamadı.";

            //degerlendirilecek urunu bul.
            var product = await _context.Product.FirstOrDefaultAsync(p => p.ProductID == rateDto.ProductId);
            if (product == null)
                return "Ürün bulunamadı.";

            //degerlendirmeyi gelen verilerle tabloya gir.
            var rating = new Rating
            {
                UserID = user.Id,
                ProductID = rateDto.ProductId,
                Score = rateDto.Score,
                Comment = rateDto.Comment,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            //bilgileri kaydet
            await _context.Rating.AddAsync(rating);
            await _context.SaveChangesAsync();

            return "Değerlendirmeniz başarıyla kaydedildi.";
        }

        //belli bir urune ait tum degerlendirmeleri listeler.
        public async Task<List<Rating>> GetRateByProductAsync(int productId)
        {
            //Rating tablosundan ilgili productId ye ait urunun degerlendirmesini liste halinde dondur.
            return await _context.Rating
                .Where(r => r.ProductID == productId)
                .Include(r => r.User)
                .ToListAsync();
        }

        //saticinin urununu istedigi kampanyayla eslestirmesini saglar.
        public async Task<bool> AssignProductToCampaign(int productId, string campaignName, string userEmail)
        {
            //kullanici satici mi 
            var user = await _context.Users
             .Include(u => u.Role)
             .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                throw new KeyNotFoundException("Kullanıcı bulunamadı.");
            }

            // 2️ Rolünü kontrol et
            if (user.RoleId != 2) // Eğer role 2 değilse yetkisiz
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok.");
            }

            //urun secilir
            var product = await _context.Product
                .Include(p => p.ProductCampaign)
                .FirstOrDefaultAsync(p => p.ProductID == productId);

            if (product == null)
            {
                throw new KeyNotFoundException("Ürün bulunamadı.");
            }

            //kampanya secilir
            var campaign = await _context.Campaign
                .FirstOrDefaultAsync(c => c.CampaignName == campaignName);

            if (campaign == null)
            {
                throw new KeyNotFoundException("Kampanya bulunamadı.");
            }

            // Eğer ürün zaten bu kampanyaya ekliyse tekrar ekleme
            if (product.ProductCampaign.Any(pc => pc.CampaignID == campaign.CampaignID))
            {
                throw new InvalidOperationException("Ürün zaten bu kampanyaya eklenmiş.");
            }

            var productCampaign = new ProductCampaign
            {
                ProductID = productId,
                CampaignID = campaign.CampaignID
            };

            //urunleri ve secilen kampanyayi db ye ekle
            _context.ProductCampaign.Add(productCampaign);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<ProductDto>> GetProductsByCampaign(string campaignName)
        {
            //secili kampanyayi bul
            var campaign = await _context.Campaign
                .FirstOrDefaultAsync(c => c.CampaignName == campaignName);

            if (campaign == null)
            {
                throw new KeyNotFoundException("Kampanya bulunamadı.");
            }

            //o kampanyaya ozgu urunleri bul
            var products = await _context.ProductCampaign
                .Where(pc => pc.CampaignID == campaign.CampaignID)
                .Select(pc => new ProductDto
                {
                    ProductID = pc.Product.ProductID,
                    ProductName = pc.Product.ProductName,
                    ProductImage = pc.Product.ProductImage,
                    Price = pc.Product.Price,
                    Stock = pc.Product.Stock
                })
                .ToListAsync();

            //urunleri dondur
            return products;
        }
    }
}
