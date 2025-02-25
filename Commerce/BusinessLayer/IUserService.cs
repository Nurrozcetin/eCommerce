using Commerce.EntityLayer.Dtos;
using Commerce.EntityLayer.Models;

namespace Commerce.BusinessLayer
{
    public interface IUserService
    {
        Task<AuthResponseDto> RegisterAsync(int userId, string email, string password, int? genderId, int? roleId);
        Task<AuthResponseDto> LoginAsync(string email, string password);
        Task<User> ValidateUserAsync(int userId, string password);
        Task<ProfileDto> GetUserByIdAsync(int userId); //kullaniciya ait kisisel bilgileri ceker.
        Task<string> UpdateUserAsync(int userId, UpdateProfileDto updateProfileDto); //kullanicinin kisisel bilgilerini guncelleyebilmesini saglar.
    }
}
