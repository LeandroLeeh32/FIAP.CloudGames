namespace FIAP.CloudGames.Application.UseCases.Games
{
    public interface IUpdateGameUseCase
    {
        Task<GameResult<GameDto>> ExecuteAsync(Guid id, UpdateGameInput input);
    }
}
