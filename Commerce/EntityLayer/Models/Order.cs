using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserID { get; set; }
        public User User { get; set; }

        public List<OrderItem> OrderItems { get; set; } = [];

        public int StatusID { get; set; } = 1;
        public Status Status { get; set; }

        public Payment Payment { get; set; }

        public DateTime DeliveryDate { get; set; }
    }
}
