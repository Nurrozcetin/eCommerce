using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class ProductCampaign
    {
        [Key]
        public int ProductCampaignID { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int CampaignID { get; set; }
        public Campaign Campaign { get; set; }
    }
}
