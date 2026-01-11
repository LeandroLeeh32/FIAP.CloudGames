using FIAP.CloudGames.Domain.Enums;

namespace FIAP.CloudGames.API.Controllers.DTOs.Requests.Users
{
    public class CreateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
