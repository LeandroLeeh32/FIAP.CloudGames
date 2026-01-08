using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Application.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(User user);
    }
}
