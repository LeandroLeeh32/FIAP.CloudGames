using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Application.UseCases.Users;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FIAP.CloudGames.Tests.Application.UseCases.Users;

public class UsersUseCasesTests
{
    [Fact]
    public async Task ExecuteAsync_GetUsers_ReturnsUsers()
    {
        var users = new List<User>
        {
            User.Create("User One", "one@example.com", "hash", UserRole.User),
            User.Create("User Two", "two@example.com", "hash", UserRole.User)
        };

        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        var sut = new GetUsersUseCase(
            repository.Object,
            new Mock<ILogger<GetUsersUseCase>>().Object);

        var result = await sut.ExecuteAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ExecuteAsync_GetUserById_ReturnsUser()
    {
        var user = User.Create("User", "user@example.com", "hash", UserRole.User);

        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var sut = new GetUserByIdUseCase(
            repository.Object,
            new Mock<ILogger<GetUserByIdUseCase>>().Object);

        var result = await sut.ExecuteAsync(user.Id);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result?.Id);
    }

    [Fact]
    public async Task ExecuteAsync_UpdateUser_UpdatesUser()
    {
        var user = User.Create("User", "user@example.com", "hash", UserRole.User);

        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        repository.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

        var sut = new UpdateUserUseCase(
            repository.Object,
            new Mock<ILogger<UpdateUserUseCase>>().Object);

        var result = await sut.ExecuteAsync(
            user.Id,
            "Updated User",
            "updated@example.com",
            UserRole.Admin);

        Assert.True(result.Success);
        Assert.Equal("Updated User", user.Name);
        Assert.Equal("updated@example.com", user.Email);
        Assert.Equal(UserRole.Admin, user.Role);
    }

    [Fact]
    public async Task ExecuteAsync_DeleteUser_RemovesUser()
    {
        var user = User.Create("User", "user@example.com", "hash", UserRole.User);

        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        repository.Setup(r => r.DeleteAsync(user)).Returns(Task.CompletedTask);

        var sut = new DeleteUserUseCase(
            repository.Object,
            new Mock<ILogger<DeleteUserUseCase>>().Object);

        var result = await sut.ExecuteAsync(user.Id);

        Assert.True(result.Success);
        repository.Verify(r => r.DeleteAsync(user), Times.Once);
    }
}
