using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class ProductSize //many to many iliski 
    {
        public int ProductSizeID { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int SizeId { get; set; }
        public Size Size { get; set; }

        public int Stock { get; set; }
    }
}
