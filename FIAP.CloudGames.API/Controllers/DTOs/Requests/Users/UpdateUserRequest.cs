using FIAP.CloudGames.Domain.Enums;

namespace FIAP.CloudGames.API.Controllers.DTOs.Requests.Users
{
    public class UpdateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
