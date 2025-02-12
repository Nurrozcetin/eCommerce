using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Addresses
    {
        [Key]
        public int AddressID { get; set; }
        public required string Title { get; set; }
        public required string Address { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }
    }
}
