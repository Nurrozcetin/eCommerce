using Commerce.EntityLayer.Dtos;
using Commerce.EntityLayer.Models;

namespace Commerce.BusinessLayer
{
    public interface IUserService
    {
        Task<string> RegisterAsync(string email, string password, int? genderId, int? roleId);
        Task<User> LoginAsync(string email, string password);
        Task<User> ValidateUserAsync(string email, string password);
        Task<ProfileDto> GetUserByEmailAsync(string email); //kullaniciya ait kisisel bilgileri ceker.
        Task<string> UpdateUserAsync(int userId, UpdateProfileDto updateProfileDto); //kullanicinin kisisel bilgilerini guncelleyebilmesini saglar.
    }
}
