using Commerce.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using BCrypt.Net;
using Commerce.EntityLayer.Models;
using Commerce.EntityLayer.Dtos;

namespace Commerce.BusinessLayer
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context; //data access i kaldirip tum db islemlerini de business de yapacagimiz icin dbcontext i buraya implemente ediyoruz

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> RegisterAsync(string email, string password, int? genderId, int? roleId)
        {
            //epostayla kullanicinin varligi sorgulanir
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return "Bu e-posta zaten kullanılıyor.";
            }

            // kullanici yoksa yeni sifreyi hashliyoruz
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Email = email,
                Password = hashedPassword,
                GenderId = genderId ?? 3,
                RoleId = roleId ?? 1
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");
            return user;
        }

        public async Task<ProfileDto> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Gender)
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new Exception("Bu maile kayıtlı bir kullanıcı bulunamadı");

            var profileDto = new ProfileDto
            {
                Email = user.Email,
                Name = user.Name ?? string.Empty,
                TelNo = user.TelNo ?? string.Empty,
                Birthday = user.Birthday,
                Gender = user.Gender?.Name ?? string.Empty,
                Role = user.Role?.RoleName ?? string.Empty,
                Addresses = user.Addresses?.Select(a => a.Address ?? string.Empty).ToList() ?? new List<string>()
            };

            return profileDto;
        }

        public async Task<string> UpdateUserAsync(int userId, UpdateProfileDto updateProfileDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if ( user == null)
            {
                return "İlgili kullanıcı bulunamadı.";
            }

            // email update
            if ( updateProfileDto.Email != null )
            {
                var email = await _context.Users.FirstOrDefaultAsync(u => u.Email == updateProfileDto.Email);
                if ( email != null )
                {
                    return "Bu e-posta zaten kullanılıyor.";
                }
                user.Email = updateProfileDto.Email;
            }

            //sifre update
            if (updateProfileDto.Password != null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(updateProfileDto.Password);
            }

            //isim update
            if (updateProfileDto.Name != null)
                user.Name = updateProfileDto.Name;

            //tel no update
            if (updateProfileDto.TelNo != null)
                user.TelNo = updateProfileDto.TelNo;

            //dg update
            if (updateProfileDto.Birthday != null)
                user.Birthday = updateProfileDto.Birthday;


            //rol update
            if (updateProfileDto.RoleId.HasValue)
                user.RoleId = updateProfileDto.RoleId.Value;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return "Kullanıcı bilgileri başarıyla güncellendi.";
        }

    }
}
