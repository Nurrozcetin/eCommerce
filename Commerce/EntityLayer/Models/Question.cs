using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Question
    {
        [Key]
        public int QuestionID { get; set; }
        public required string Questions { get; set; }
        public required string Answer { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserID { get; set; } //soru soran kullanici id si
        public User User { get; set; }

        public int ProductID { get; set; } //hangi urunle ilgili soru soruluyor 
        public Product Product { get; set; }
    }
}
