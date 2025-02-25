using Commerce.EntityLayer.Models;

namespace Commerce.EntityLayer.Dtos
{
    public class RegisterDto
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int? GenderId { get; set; }
        public int? RoleId { get; set; }
    }
}
