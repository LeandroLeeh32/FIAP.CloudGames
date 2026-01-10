namespace FIAP.CloudGames.Application.UseCases.Games
{
    public interface IGetGameByIdUseCase
    {
        Task<GameDto?> ExecuteAsync(Guid id);
    }
}
