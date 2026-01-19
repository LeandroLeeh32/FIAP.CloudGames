namespace FIAP.CloudGames.Application.UseCases.UserGames
{
    public class UserGameResult
    {
        private UserGameResult(bool success, UserGameError error, string? message)
        {
            Success = success;
            Error = error;
            Message = message;
        }

        public bool Success { get; }
        public UserGameError Error { get; }
        public string? Message { get; }

        public static UserGameResult Ok() => new(true, UserGameError.None, null);

        public static UserGameResult Fail(UserGameError error, string message)
            => new(false, error, message);
    }
}
