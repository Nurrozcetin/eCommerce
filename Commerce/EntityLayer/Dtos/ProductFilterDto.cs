namespace Commerce.EntityLayer.Dtos
{
    public class ProductFilterDto
    {
        public string? ProductName { get; set; }
        public int? CategoryId { get; set; }
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinStock { get; set; }
        public int? MaxStock { get; set; }
    }
}
