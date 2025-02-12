using Commerce.EntityLayer.Models;

namespace Commerce.EntityLayer.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Gender? Gender { get; set; }
    }
}
