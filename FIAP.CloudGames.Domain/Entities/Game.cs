namespace FIAP.CloudGames.Domain.Entities
{
    public class Game
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public decimal Price { get; private set; }

        protected Game() { } // EF

        public Game(string title, decimal price, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Título inválido");

            if (price < 0)
                throw new ArgumentException("Preço inválido");

            Id = Guid.NewGuid();
            Title = title;
            Price = price;
            Description = description;
        }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Título inválido");

            Title = title.Trim();
        }
        public void UpdateDescription(string? description)
        {
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim();
        }

        public void UpdatePrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException("Preço inválido");

            Price = price;
        }

        public ICollection<UserGame> UserGames { get; private set; }
            = new List<UserGame>();

        public ICollection<PromotionGame> PromotionGames { get; private set; }
            = new List<PromotionGame>();
    }
}
