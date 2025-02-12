using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Status //pending, shipped, delivered, canceled, completed, failed
    {
        [Key]
        public int StatusID { get; set; }

        [Required]
        public string StatusName { get; set; }

        public Order Order { get; set; }

        public Payment Payment { get; set; }

    }
}
