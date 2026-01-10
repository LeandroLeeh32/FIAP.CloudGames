using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> _users = new();

        public void Add(User user)
        {
            _users.Add(user);
        }

        public void Delete(Guid id)
        {
            _users.RemoveAll(u => u.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User? GetById(Guid id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public void Update(User user)
        {
            // In-memory: o objeto já está atualizado
        }
    }
}
