namespace FIAP.CloudGames.Application.UseCases.Games
{
    public class GameResult
    {
        private GameResult(bool success, GameError error, string? message)
        {
            Success = success;
            Error = error;
            Message = message;
        }

        public bool Success { get; }
        public GameError Error { get; }
        public string? Message { get; }

        public static GameResult Ok() => new(true, GameError.None, null);

        public static GameResult Fail(GameError error, string message) => new(false, error, message);
    }
}
