namespace FIAP.CloudGames.Application.UseCases.PromotionGames
{
    public class PromotionGameResult
    {
        private PromotionGameResult(bool success, PromotionGameError error, string? message)
        {
            Success = success;
            Error = error;
            Message = message;
        }

        public bool Success { get; }
        public PromotionGameError Error { get; }
        public string? Message { get; }

        public static PromotionGameResult Ok() => new(true, PromotionGameError.None, null);

        public static PromotionGameResult Fail(PromotionGameError error, string message)
            => new(false, error, message);
    }
}
