using FIAP.CloudGames.Application.Interfaces.Services;
using FIAP.CloudGames.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace FIAP.CloudGames.Infrastructure.Security;

public class JwtTokenService : ITokenService
{
    private const string SecretKey = "FIAP_CLOUD_GAMES_SUPER_SECRET_KEY_123456_123456";
    private const string Issuer = "FIAP.CloudGames";
    private const string Audience = "FIAP.CloudGames";

    private readonly ILogger<JwtTokenService> _logger;
    public JwtTokenService(ILogger<JwtTokenService> logger)
    {
        _logger = logger;
    }

    public string GenerateToken(User user)
    {
        _logger.LogInformation("[Infra][JwtTokenService] Iniciando geração de token JWT para UsuárioId={UsuarioId} Perfil={Perfil}", user.Id, user.Role);
        try
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

            _logger.LogInformation("[Infra][JwtTokenService] Token JWT gerado com sucesso para UsuárioId={UsuarioId}",user.Id);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError( ex, "[Infra][JwtTokenService] Erro ao gerar token JWT para UsuárioId={UsuarioId}",user.Id);
            throw; 
          
        }
       
    }
}
