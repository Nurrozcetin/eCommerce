using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class User //bir kullanicinin olusmasi icin gerekli olan entity ler
    {
        [Key]
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Name { get; set; }
        public string? TelNo { get; set; }
        public DateOnly? Birthday { get; set; }

        public int GenderId { get; set; }
        public Gender? Gender { get; set; } //cinsiyeti optional tutup ya kadin ya erkek secilmesini sagliyoruz
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public Cart Cart { get; set; }
        public List<Order> Order { get; set; }
        public List<Question> Questions { get; set; } = [];
        public List<Favourites> Favourites { get; set; }
        public List<Rating> Ratings { get; set; } = [];
        public List<Product>? Products { get; set; } = [];
        public List<Addresses> Addresses { get; set; } = [];
    }
}
