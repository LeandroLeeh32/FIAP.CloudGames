namespace FIAP.CloudGames.Domain.Entities
{
    public class Promotion
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<PromotionGame> PromotionGames { get; set; } = new List<PromotionGame>();
    }
}
