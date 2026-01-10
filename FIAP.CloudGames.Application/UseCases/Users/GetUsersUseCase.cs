using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Application.UseCases.User;
public class GetUsersUseCase
{
    private readonly IUserRepository _repository;

    public GetUsersUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Domain.Entities.User> Execute()
        => _repository.GetAll();
}
