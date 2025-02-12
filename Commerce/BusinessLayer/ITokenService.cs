namespace Commerce.BusinessLayer
{
    public interface ITokenService
    {
        string GenerateToken(string email);
    }
}
