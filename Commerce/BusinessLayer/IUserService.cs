using Commerce.EntityLayer.Models;

namespace Commerce.BusinessLayer
{
    public interface IUserService
    {
        Task<string> RegisterAsync(string email, string password, Gender? gender);
        Task<User> LoginAsync(string email, string password);
        Task<User> ValidateUserAsync(string email, string password);
    }
}
