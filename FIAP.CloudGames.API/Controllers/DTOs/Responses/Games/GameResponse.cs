namespace FIAP.CloudGames.API.Controllers.DTOs.Responses.Games
{
    public class GameResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
