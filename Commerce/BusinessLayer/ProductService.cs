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
        public async Task<List<Product>> FilterProductsAsync(ProductFilterDto filterDto)
        {
            var query = _context.Product.AsQueryable();

            if (!string.IsNullOrEmpty(filterDto.ProductName))
            {
                query = query.Where(product => product.ProductName.Contains(filterDto.ProductName));
            }

            if (filterDto.CategoryId.HasValue)
            {
                query = query.Where(product => product.Categories.Any(category => category.CategoryID == filterDto.CategoryId));
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
    }
}
