using Commerce.BusinessLayer;
using System.Security.Cryptography;
using System.Text;

public class TokenService : ITokenService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiryMinutes;

    // Constructor üzerinden IConfiguration alıyoruz
    public TokenService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:Key"];  // appsettings.json'dan alınır
        _issuer = configuration["Jwt:Issuer"];
        _audience = configuration["Jwt:Audience"];
        _expiryMinutes = int.Parse(configuration["Jwt:ExpiryMinutes"]);
    }

    public string GenerateToken(int userId)
    {
        var header = new { alg = "HS256", typ = "JWT" };
        string encodedHeader = Base64UrlEncode(System.Text.Json.JsonSerializer.Serialize(header));

        var payload = new
        {
            sub = userId.ToString(),
            name = "username",
            exp = DateTimeOffset.UtcNow.AddMinutes(_expiryMinutes).ToUnixTimeSeconds()  // ExpiryMinutes dinamik
        };
        string encodedPayload = Base64UrlEncode(System.Text.Json.JsonSerializer.Serialize(payload));

        string signature = CreateSignature($"{encodedHeader}.{encodedPayload}", _secretKey);

        return $"{encodedHeader}.{encodedPayload}.{signature}";
    }

    private string Base64UrlEncode(byte[] bytes)
    {
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private string Base64UrlEncode(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        return Base64UrlEncode(bytes);
    }

    private string CreateSignature(string message, string secretKey)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
        {
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return Base64UrlEncode(hash);
        }
    }
}