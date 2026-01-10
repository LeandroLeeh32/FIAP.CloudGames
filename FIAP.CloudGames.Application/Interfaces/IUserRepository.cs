using FIAP.CloudGames.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.CloudGames.Application.Interfaces
{
    public interface IUserRepository
    {
        void Add(Users user);
        Users? GetById(Guid id);
        IEnumerable<Users> GetAll();
        void Update(Users user);
        void Delete(Guid id);
    }
}
