using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Rating
    {
        [Key]
        public int RateID { get; set; }
        public required string Comment { get; set; }
        public DateOnly CreatedAt { get; set; }
        public Size? Size { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }
    }
}
