using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Dtos
{
    public class RateDto
    {
        public int UserId { get; set; }
        public required int ProductId { get; set; }
        [Range(1, 5, ErrorMessage = "Puan 1 ile 5 arasında olmalıdır.")]
        public int Score { get; set; } // 1-5 arası puan
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
