using System.Globalization;

namespace Commerce.EntityLayer.Dtos
{
    public class ProfileDto
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public string? Name { get; set; }
        public string? TelNo { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? Gender { get; set; }
        public string? Role {  get; set; }
        public List<string?> Addresses { get; set; }
    }
}
