using Microsoft.IdentityModel.Tokens;

namespace Commerce.BusinessLayer
{
    public interface ITokenService
    {
        string GenerateToken(int userId);
    }
}
