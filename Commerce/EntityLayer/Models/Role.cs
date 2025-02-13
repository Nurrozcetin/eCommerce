using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public required string RoleName { get; set; }

        public List<User> Users { get; set; } = new List<User>();
    }
}
