using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        User? GetById(Guid id);
        IEnumerable<User> GetAll();
        void Update(User user);
        void Delete(Guid id);
    }
}
