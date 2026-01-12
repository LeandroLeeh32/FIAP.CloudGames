namespace FIAP.CloudGames.Domain.Entities
{
    public class UserGame
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;

        public Guid GameId { get; private set; }
        public Game Game { get; private set; } = null!;

        public DateTime PurchasedAt { get; private set; }

        protected UserGame() { } // EF

        public UserGame(User user, Game game)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            Game = game ?? throw new ArgumentNullException(nameof(game));

            UserId = user.Id;
            GameId = game.Id;
            PurchasedAt = DateTime.UtcNow;
        }

        public UserGame(Guid userId, Guid gameId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required.", nameof(userId));

            if (gameId == Guid.Empty)
                throw new ArgumentException("GameId is required.", nameof(gameId));

            UserId = userId;
            GameId = gameId;
            PurchasedAt = DateTime.UtcNow;
        }
    }
}
