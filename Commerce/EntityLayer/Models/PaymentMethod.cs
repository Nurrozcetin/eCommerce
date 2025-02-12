using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class PaymentMethod //kredi karti, paypal, havale
    {
        [Key]
        public int PaymentMethodID { get; set; }

        public required string PaymentMethodName { get; set; }
    }
}
