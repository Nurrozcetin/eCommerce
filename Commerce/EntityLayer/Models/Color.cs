using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Color
    {
        public int ColorID { get; set; }
        public string ColorName { get; set; }

        public List<ProductColor> ProductColor { get; set; } = [];
    }
}
