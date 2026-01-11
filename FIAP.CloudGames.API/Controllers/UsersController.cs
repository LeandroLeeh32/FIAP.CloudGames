using FIAP.CloudGames.API.Controllers.DTOs.Requests.Users;
using FIAP.CloudGames.Application.UseCases.Users;
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
    private readonly GetUserByIdUseCase _getById;

    public UsersController(CreateUserUseCase create,GetUsersUseCase get,UpdateUserUseCase update,DeleteUserUseCase delete, GetUserByIdUseCase getById)
    {
        _create = create;
        _get = get;
        _update = update;
        _delete = delete;
        _getById = getById;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var id = await _create.ExecuteAsync(request.Name,request.Email, request.Password, request.Role);
        return CreatedAtAction(nameof(GetAll),new { id },null);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _delete.ExecuteAsync(id);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _get.ExecuteAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _getById.ExecuteAsync(id);
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id,[FromBody] UpdateUserRequest request)
    {
        await _update.ExecuteAsync(id,request.Name,request.Email,request.Role);
        return NoContent();
    }



}
