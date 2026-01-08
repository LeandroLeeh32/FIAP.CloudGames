using System.Security.Claims;
using System.Text;
using FIAP.CloudGames.Application.Interfaces;
using FIAP.CloudGames.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace FIAP.CloudGames.Infrastructure.Security;

public class JwtTokenService : IAuthService
{
    private const string SecretKey = "FIAP_CLOUD_GAMES_SUPER_SECRET_KEY_123456_123456";
    private const string Issuer = "FIAP.CloudGames";
    private const string Audience = "FIAP.CloudGames";

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
