using FIAP.CloudGames.API.Contracts.Users;
using FIAP.CloudGames.Application.UseCases.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly CreateUserUseCase _create;
    private readonly GetUsersUseCase _get;
    private readonly UpdateUserUseCase _update;
    private readonly DeleteUserUseCase _delete;

    public UsersController(
        CreateUserUseCase create,
        GetUsersUseCase get,
        UpdateUserUseCase update,
        DeleteUserUseCase delete)
    {
        _create = create;
        _get = get;
        _update = update;
        _delete = delete;
    }

    [HttpPost]
    public IActionResult Create(CreateUserRequest request)
    {
        var id = _create.Execute(
            request.Name,
            request.Email,
            request.Role);

        return CreatedAtAction(nameof(GetAll), new { id }, null);
    }

    [HttpGet]
    public IActionResult GetAll()
        => Ok(_get.Execute());

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, UpdateUserRequest request)
    {
        _update.Execute(id, request.Name, request.Email, request.Role);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _delete.Execute(id);
        return NoContent();
    }
}
