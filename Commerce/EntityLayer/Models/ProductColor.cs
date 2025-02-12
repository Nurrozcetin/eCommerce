using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class ProductColor
    {
        public int ProductColorID { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ColorId { get; set; }
        public Color Color { get; set; }

        public int Stock { get; set; }
    }
}