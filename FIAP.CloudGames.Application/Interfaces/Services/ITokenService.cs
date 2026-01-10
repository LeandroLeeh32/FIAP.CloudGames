using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
