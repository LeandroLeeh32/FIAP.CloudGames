namespace FIAP.CloudGames.Domain.Entities
{
    public class Promotion
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public decimal DiscountPercentage { get; private set; }
        public DateTime StartsAt { get; private set; }
        public DateTime EndsAt { get; private set; }
        public bool IsActive { get; private set; }

        protected Promotion() { }

        public Promotion(
            string name,
            decimal discountPercentage,
            DateTime startsAt,
            DateTime endsAt,
            string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome inválido");

            if (discountPercentage <= 0 || discountPercentage > 100)
                throw new ArgumentException("Desconto inválido");

            if (endsAt <= startsAt)
                throw new ArgumentException("Período inválido");

            Id = Guid.NewGuid();
            Name = name;
            DiscountPercentage = discountPercentage;
            StartsAt = startsAt;
            EndsAt = endsAt;
            Description = description;
            IsActive = true;
        }

        public ICollection<PromotionGame> PromotionGames { get; private set; }
            = new List<PromotionGame>();
    }
}
