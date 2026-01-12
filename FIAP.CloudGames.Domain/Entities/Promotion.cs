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
            Validate(name, discountPercentage, startsAt, endsAt);

            Id = Guid.NewGuid();
            Name = name.Trim();
            DiscountPercentage = discountPercentage;
            StartsAt = startsAt;
            EndsAt = endsAt;
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim();
            IsActive = true;
        }

        public void Update(
            string name,
            decimal discountPercentage,
            DateTime startsAt,
            DateTime endsAt,
            string? description,
            bool isActive)
        {
            Validate(name, discountPercentage, startsAt, endsAt);

            Name = name.Trim();
            DiscountPercentage = discountPercentage;
            StartsAt = startsAt;
            EndsAt = endsAt;
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim();
            IsActive = isActive;
        }

        private static void Validate(
            string name,
            decimal discountPercentage,
            DateTime startsAt,
            DateTime endsAt)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome invalido");

            if (discountPercentage <= 0 || discountPercentage > 100)
                throw new ArgumentException("Desconto invalido");

            if (endsAt <= startsAt)
                throw new ArgumentException("Periodo invalido");
        }

        public ICollection<PromotionGame> PromotionGames { get; private set; }
            = new List<PromotionGame>();
    }
}
