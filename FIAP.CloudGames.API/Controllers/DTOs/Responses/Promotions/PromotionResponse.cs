namespace FIAP.CloudGames.API.Controllers.DTOs.Responses.Promotions
{
    public class PromotionResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public bool IsActive { get; set; }
        public IReadOnlyList<Guid> GameIds { get; set; } = new List<Guid>();
    }
}
