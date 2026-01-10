namespace FIAP.CloudGames.Domain.Entities
{
    public class UserGame
    {
        public Guid UserId { get; set; }
        public Users User { get; set; } = null!;

        public Guid GameId { get; set; }
        public Game Game { get; set; } = null!;

        public DateTime PurchasedAt { get; set; }
    }
}
