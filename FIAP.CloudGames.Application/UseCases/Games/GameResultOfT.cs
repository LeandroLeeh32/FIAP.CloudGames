namespace FIAP.CloudGames.Application.UseCases.Games
{
    public class GameResult<T>
    {
        private GameResult(bool success, GameError error, string? message, T? data)
        {
            Success = success;
            Error = error;
            Message = message;
            Data = data;
        }

        public bool Success { get; }
        public GameError Error { get; }
        public string? Message { get; }
        public T? Data { get; }

        public static GameResult<T> Ok(T data) => new(true, GameError.None, null, data);

        public static GameResult<T> Fail(GameError error, string message) => new(false, error, message, default);
    }
}
