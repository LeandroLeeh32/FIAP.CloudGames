using FIAP.CloudGames.Domain.Enums;

namespace FIAP.CloudGames.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;


        // MOCK: senha não persistida ainda
        public User(string name, string email, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome inválido");

            if (!email.Contains("@"))
                throw new ArgumentException("E-mail inválido");

            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Role = role;
        }

        public void Update(string name, string email, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome inválido");

            if (!email.Contains("@"))
                throw new ArgumentException("E-mail inválido");

            Name = name;
            Email = email;
            Role = role;
        }

        public ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();

    }
}
