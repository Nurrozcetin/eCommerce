namespace Commerce.EntityLayer.Dtos
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerEmail { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
