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
        public DateOnly Date { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }
    }
}
