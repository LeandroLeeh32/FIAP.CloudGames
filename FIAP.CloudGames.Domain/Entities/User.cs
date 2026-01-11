using FIAP.CloudGames.Domain.Enums;

namespace FIAP.CloudGames.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public UserRole Role { get; private set; }

        // NÃO persistir senha no Domain agora
        // Será tratada no futuro (Identity, Auth, etc.)

        protected User() { } // EF Core

        public User(string name, string email, UserRole role)
        {
            Validate(name, email);

            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Role = role;
        }

        public void Update(string name, string email, UserRole role)
        {
            Validate(name, email);

            Name = name;
            Email = email;
            Role = role;
        }

        private static void Validate(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome inválido");

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("E-mail inválido");
        }

        public ICollection<UserGame> UserGames { get; private set; }
            = new List<UserGame>();
    }
}
