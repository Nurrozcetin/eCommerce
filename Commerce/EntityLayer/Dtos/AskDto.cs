namespace Commerce.EntityLayer.Dtos
{
    public class AskDto
    {
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public int SellerId { get; set; } 
        public required int ProductId{ get; set; }
        public string? Content { get; set; }
        public string? Answers { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
