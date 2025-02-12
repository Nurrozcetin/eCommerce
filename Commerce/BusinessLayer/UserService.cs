using Commerce.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using BCrypt.Net;
using Commerce.EntityLayer.Models;

namespace Commerce.BusinessLayer
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context; //data access i kaldirip tum db islemlerini de business de yapacagimiz icin dbcontext i buraya implemente ediyoruz

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> RegisterAsync(string email, string password, Gender? gender)
        {
            //epostayla kullanicinin varligi sorgulanir
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return "Bu e-posta adresiyle zaten bir hesap mevcut!";

            // kullanici yoksa yeni sifreyi hashliyoruz
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Email = email,
                Password = hashedPassword,
                Gender = gender
            };

            // kullaniciyi db ye ekliyoruz
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return "Kullanıcı başarıyla oluşturuldu!";
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new Exception("Şifre hatalı!");

            return user;
        }

        public async Task<User> ValidateUserAsync(string email, string password)
        {
            // Burada password hashing vs. gibi işlemler yapılabilir
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password); // Burada şifreyi doğru şekilde kontrol etmelisiniz
        }
    }
}
