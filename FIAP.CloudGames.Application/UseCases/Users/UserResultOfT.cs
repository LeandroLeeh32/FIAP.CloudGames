namespace FIAP.CloudGames.Application.UseCases.Users;

public class UserResult<T>
{
    private UserResult(bool success, UserError error, string? message, T? data)
    {
        Success = success;
        Error = error;
        Message = message;
        Data = data;
    }

    public bool Success { get; }
    public UserError Error { get; }
    public string? Message { get; }
    public T? Data { get; }

    public static UserResult<T> Ok(T data) => new(true, UserError.None, null, data);

    public static UserResult<T> Fail(UserError error, string message)
        => new(false, error, message, default);
}
