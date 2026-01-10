using FIAP.CloudGames.Application.Interfaces;
using FIAP.CloudGames.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.CloudGames.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private static readonly List<Users> _users = new();

        public void Add(Users user)
        {
            _users.Add(user);
        }

        public void Delete(Guid id)
        {
            _users.RemoveAll(u => u.Id == id);
        }

        public IEnumerable<Users> GetAll()
        {
            return _users;
        }

        public Users? GetById(Guid id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public void Update(Users user)
        {
            // In-memory: o objeto já está atualizado
        }
    }
}
