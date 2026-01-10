namespace FIAP.CloudGames.Application.UseCases.Games
{
    public class CreateGameInput
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
