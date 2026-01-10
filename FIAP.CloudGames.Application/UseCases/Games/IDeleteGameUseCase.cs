namespace FIAP.CloudGames.Application.UseCases.Games
{
    public interface IDeleteGameUseCase
    {
        Task<GameResult> ExecuteAsync(Guid id);
    }
}
