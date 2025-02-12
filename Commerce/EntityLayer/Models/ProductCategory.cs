namespace Commerce.EntityLayer.Models
{
    public class ProductCategory
    {
        public int ProductCategoryID { get; set; }
        public int Stock { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int CategoryId { get; set; }
        public Categories Categories { get; set; }
    }
}
