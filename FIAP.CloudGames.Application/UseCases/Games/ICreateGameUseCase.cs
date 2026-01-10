namespace FIAP.CloudGames.Application.UseCases.Games
{
    public interface ICreateGameUseCase
    {
        Task<GameResult<GameDto>> ExecuteAsync(CreateGameInput input);
    }
}
