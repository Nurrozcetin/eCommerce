using Commerce.EntityLayer.Models;

namespace Commerce.EntityLayer.Dtos
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public required string ProductName { get; set; }
        public required string ProductImage { get; set; }
        public required decimal Price { get; set; }
        public int? Stock { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? SellerName { get; set; }
    }
}

