using FIAP.CloudGames.Domain.Enums;

namespace FIAP.CloudGames.Domain.Entities
{
    public class User
    {
     
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public UserRole Role { get; private set; }

        protected User() { } // EF Core

        private User(string name, string email, string passwordHash, UserRole role)
        {
            Validate(name, email);

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("[Domain][User] Senha inválida");

            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }

   
        public static User Create(string name, string email, string passwordHash, UserRole role)
        {
            if(string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Senha inválida");

            return new User(name, email, passwordHash, role);
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
                throw new ArgumentException("[Domain][User] Nome inválido");

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("[Domain][User] E-mail inválido");
        }

        public ICollection<UserGame> UserGames { get; private set; }
            = new List<UserGame>();
    }
}
