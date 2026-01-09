namespace FIAP.CloudGames.API.Controllers.DTOs.Requests.Games
{
    public class UpdateGameRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
