namespace FIAP.CloudGames.Application.UseCases.Promotions
{
    public class PromotionResult
    {
        private PromotionResult(bool success, PromotionError error, string? message)
        {
            Success = success;
            Error = error;
            Message = message;
        }

        public bool Success { get; }
        public PromotionError Error { get; }
        public string? Message { get; }

        public static PromotionResult Ok() => new(true, PromotionError.None, null);

        public static PromotionResult Fail(PromotionError error, string message)
            => new(false, error, message);
    }
}
