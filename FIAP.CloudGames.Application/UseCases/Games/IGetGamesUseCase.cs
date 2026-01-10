namespace FIAP.CloudGames.Application.UseCases.Games
{
    public interface IGetGamesUseCase
    {
        Task<IReadOnlyList<GameDto>> ExecuteAsync();
    }
}
