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
                query = query.Where(product => decimal.Parse(product.Price) >= filterDto.MinPrice);
            }

            if (filterDto.MaxPrice.HasValue)
            {
                query = query.Where(product => decimal.Parse(product.Price) <= filterDto.MaxPrice);
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
                SellerName = product.Seller != null ? product.Seller.Name : "Bilinmiyor"
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
    }
}
