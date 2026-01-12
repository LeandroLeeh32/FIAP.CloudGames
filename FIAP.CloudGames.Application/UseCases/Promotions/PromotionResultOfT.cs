namespace FIAP.CloudGames.Application.UseCases.Promotions
{
    public class PromotionResult<T>
    {
        private PromotionResult(bool success, PromotionError error, string? message, T? data)
        {
            Success = success;
            Error = error;
            Message = message;
            Data = data;
        }

        public bool Success { get; }
        public PromotionError Error { get; }
        public string? Message { get; }
        public T? Data { get; }

        public static PromotionResult<T> Ok(T data) => new(true, PromotionError.None, null, data);

        public static PromotionResult<T> Fail(PromotionError error, string message)
            => new(false, error, message, default);
    }
}
