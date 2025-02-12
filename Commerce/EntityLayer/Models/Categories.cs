using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Categories
    {
        [Key]
        public int CategoryID { get; set; }
        public required string CategoryName { get; set; }

        public List<ProductCategory> ProductCategory { get; set; } = [];
    }
}
