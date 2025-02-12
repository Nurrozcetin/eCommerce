using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        public decimal Amount { get; set; }

        public int OrderID { get; set; }
        public Order Order { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public int StatusID { get; set; }
        public Status Status { get; set; }
    }
}
