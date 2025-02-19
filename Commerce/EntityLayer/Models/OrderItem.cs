using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class OrderItem
    {
        [Key]
        public int ItemID { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int OrderID { get; set; }
        public Order Order { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int StatusID { get; set; } = 1;
        public Status Status { get; set; }
    }
}
