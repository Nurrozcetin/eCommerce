using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Rating
    {
        [Key]
        public int RateID { get; set; }

        [Range(1, 5, ErrorMessage = "Puan 1 ile 5 arasında olmalıdır.")]
        public int Score { get; set; }
        public string? Comment { get; set; }
        public DateOnly CreatedAt { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
