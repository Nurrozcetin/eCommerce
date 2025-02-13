using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Gender
    {
        public int GenderID { get; set; }
        public required string Name { get; set; }
    }
}
