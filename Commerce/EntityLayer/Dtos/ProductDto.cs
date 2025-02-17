using Commerce.EntityLayer.Models;

namespace Commerce.EntityLayer.Dtos
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public required string ProductName { get; set; }
        public required string ProductImage { get; set; }
        public required string Price { get; set; }
        public string? Stock { get; set; }
        public DateOnly CreatedAt { get; set; }
        public string? SellerName { get; set; }
    }
}

