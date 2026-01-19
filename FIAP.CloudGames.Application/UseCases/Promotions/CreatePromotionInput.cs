namespace FIAP.CloudGames.Application.UseCases.Promotions
{
    public class CreatePromotionInput
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
    }
}
