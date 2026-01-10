using FIAP.CloudGames.Application.Interfaces;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.User
{
    public class CreateUserUseCase
    {

        private readonly IUserRepository _repository;
        private readonly ILogger<CreateUserUseCase> _logger;
        public CreateUserUseCase( IUserRepository repository,ILogger<CreateUserUseCase> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public Guid Execute(string name, string email, UserRole role)
        {
            _logger.LogInformation(
                "[App][CreateUserUseCase] Criando usuário com perfil {Perfil}",
                role);

            var user = new Users(name, email, role);
            _repository.Add(user);

            return user.Id;
        }


    }
}
