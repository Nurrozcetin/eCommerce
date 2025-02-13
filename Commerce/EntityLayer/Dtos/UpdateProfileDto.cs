namespace Commerce.EntityLayer.Dtos
{
    //kullanicinin kisisel bilgilerini guncelleyebilmesi icin dto
    public class UpdateProfileDto
    {
        public string? Name { get; set; }
        public string? TelNo { get; set; }
        public DateOnly? Birthday { get; set; }
        public int? RoleId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; } 
    }

}
