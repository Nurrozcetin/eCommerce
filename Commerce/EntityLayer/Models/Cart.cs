using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public int ProductQuantity { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }
        public List<ProductCart> ProductCart { get; set; } = [];
    }
}
