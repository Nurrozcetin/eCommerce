using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
}
