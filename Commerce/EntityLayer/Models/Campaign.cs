using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Campaign
    {
        [Key]
        public int CampaignID { get; set; }

        public required string CampaignName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<ProductCampaign> ProductCampaigns { get; set; } = [];
    }
}
