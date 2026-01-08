using FIAP.CloudGames.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.CloudGames.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public UserRole Role { get; private set; }

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
    }
}
