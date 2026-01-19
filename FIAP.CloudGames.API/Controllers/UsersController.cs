using FIAP.CloudGames.API.Controllers.DTOs.Requests.Users;
using FIAP.CloudGames.Application.UseCases.Users;
using FIAP.CloudGames.Application.UseCases.UserGames;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoints for managing users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly CreateUserUseCase _create;
    private readonly GetUsersUseCase _get;
    private readonly UpdateUserUseCase _update;
    private readonly DeleteUserUseCase _delete;
    private readonly GetUserByIdUseCase _getById;
    private readonly AddGameToUserUseCase _addGameToUser;

    public UsersController(
        CreateUserUseCase create,
        GetUsersUseCase get,
        UpdateUserUseCase update,
        DeleteUserUseCase delete,
        GetUserByIdUseCase getById,
        AddGameToUserUseCase addGameToUser)
    {
        _create = create;
        _get = get;
        _update = update;
        _delete = delete;
        _getById = getById;
        _addGameToUser = addGameToUser;
    }

    /// <summary>
    /// Create a new user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var result = await _create.ExecuteAsync(request.Name, request.Email, request.Password, request.Role);

        if (!result.Success)
        {
            return result.Error switch
            {
                UserError.Validation => BadRequest(result.Message),
                UserError.Conflict => Conflict(result.Message),
                _ => BadRequest(result.Message)
            };
        }

        var id = result.Data;
        return CreatedAtAction(nameof(GetAll), new { id }, null);
    }

    /// <summary>
    /// Delete a user by id.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _delete.ExecuteAsync(id);
        if (!result.Success)
        {
            return result.Error switch
            {
                UserError.NotFound => NotFound(result.Message),
                _ => BadRequest(result.Message)
            };
        }

        return NoContent();
    }

    /// <summary>
    /// List all users.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _get.ExecuteAsync();
        return Ok(users);
    }

    /// <summary>
    /// Get a user by id.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _getById.ExecuteAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    /// <summary>
    /// Update a user by id.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        var result = await _update.ExecuteAsync(id, request.Name, request.Email, request.Role);
        if (!result.Success)
        {
            return result.Error switch
            {
                UserError.NotFound => NotFound(result.Message),
                UserError.Validation => BadRequest(result.Message),
                _ => BadRequest(result.Message)
            };
        }

        return NoContent();
    }

    /// <summary>
    /// Add a game to a user.
    /// </summary>
    [HttpPost("{id:guid}/games")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddGame(Guid id, [FromBody] AddUserGameRequest request)
    {
        var result = await _addGameToUser.ExecuteAsync(id, request.GameId);
        if (!result.Success)
        {
            return result.Error switch
            {
                UserGameError.NotFound => NotFound(result.Message),
                UserGameError.Conflict => Conflict(result.Message),
                UserGameError.Validation => BadRequest(result.Message),
                _ => BadRequest(result.Message)
            };
        }

        return NoContent();
    }
}
