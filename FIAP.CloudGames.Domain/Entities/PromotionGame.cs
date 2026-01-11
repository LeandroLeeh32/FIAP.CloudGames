namespace FIAP.CloudGames.Domain.Entities
{
    public class PromotionGame
    {
        public Guid PromotionId { get; private set; }
        public Promotion Promotion { get; private set; } = null!;
        public Guid GameId { get; private set; }
        public Game Game { get; private set; } = null!;

        protected PromotionGame() { }

        public PromotionGame(Promotion promotion, Game game)
        {
            Promotion = promotion ?? throw new ArgumentNullException(nameof(promotion));
            Game = game ?? throw new ArgumentNullException(nameof(game));

            PromotionId = promotion.Id;
            GameId = game.Id;
        }
    }
}
