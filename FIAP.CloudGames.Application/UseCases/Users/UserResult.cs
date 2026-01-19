namespace FIAP.CloudGames.Application.UseCases.Users;

public class UserResult
{
    private UserResult(bool success, UserError error, string? message)
    {
        Success = success;
        Error = error;
        Message = message;
    }

    public bool Success { get; }
    public UserError Error { get; }
    public string? Message { get; }

    public static UserResult Ok() => new(true, UserError.None, null);

    public static UserResult Fail(UserError error, string message)
        => new(false, error, message);
}
