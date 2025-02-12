using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public required string ProductName { get; set; }
        public required string ProductImage { get; set; }
        public required string Price { get; set; }
        public required string Stock { get; set; }
        public DateOnly CreatedAt { get; set; }

        public List<Categories> Categories { get; set; } = [];
        public List<OrderItem> OrderItem { get; set; } = [];
        public List<ProductCart> ProductCart { get; set; } = [];
        public List<ProductSize> ProductSizes { get; set; } = [];
        public List<ProductColor> ProductColors { get; set; } = [];
        public List<ProductCategory> ProductCategory { get; set; } = [];
        public List<ProductFavourites> ProductFavourites { get; set; } = [];
    }
}
