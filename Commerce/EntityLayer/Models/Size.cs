using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Commerce.EntityLayer.Models
{
    public class Size
    {
        [Key]
        public int SizeID { get; set; }
        public string SizeName { get; set; }
        public List<ProductSize> ProductSize { get; set; } = [];
    }
}
