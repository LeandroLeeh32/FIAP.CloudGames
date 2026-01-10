namespace FIAP.CloudGames.Domain.Entities
{
    public class PromotionGame
    {
        public Guid PromotionId { get; set; }
        public Promotion Promotion { get; set; } = null!;

        public Guid GameId { get; set; }
        public Game Game { get; set; } = null!;
    }
}
