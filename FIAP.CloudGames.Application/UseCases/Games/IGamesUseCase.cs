namespace FIAP.CloudGames.Application.UseCases.Games
{
    public interface IGamesUseCase
    {
        Task<IReadOnlyList<GameDto>> GetAllAsync();
        Task<GameDto?> GetByIdAsync(Guid id);
        Task<GameResult<GameDto>> CreateAsync(CreateGameInput input);
        Task<GameResult<GameDto>> UpdateAsync(Guid id, UpdateGameInput input);
        Task<GameResult> DeleteAsync(Guid id);
    }
}
