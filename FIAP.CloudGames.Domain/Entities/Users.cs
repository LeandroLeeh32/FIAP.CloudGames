using FIAP.CloudGames.Domain.Enums;

namespace FIAP.CloudGames.Domain.Entities
{
    public class Users
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public UserRole Role { get; private set; }

        // MOCK: senha não persistida ainda
        public Users(string name, string email, UserRole role)
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
    }
}
