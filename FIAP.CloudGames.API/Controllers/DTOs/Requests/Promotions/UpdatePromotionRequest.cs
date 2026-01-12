namespace FIAP.CloudGames.API.Controllers.DTOs.Requests.Promotions
{
    public class UpdatePromotionRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public bool IsActive { get; set; }
    }
}
