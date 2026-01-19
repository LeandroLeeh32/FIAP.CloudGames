using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Application.Repositories;
using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Application.UseCases.UserGames
{
    public class AddGameToUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;

        public AddGameToUserUseCase(
            IUserRepository userRepository,
            IGameRepository gameRepository)
        {
            _userRepository = userRepository;
            _gameRepository = gameRepository;
        }

        public async Task<UserGameResult> ExecuteAsync(Guid userId, Guid gameId)
        {
            if (userId == Guid.Empty || gameId == Guid.Empty)
            {
                return UserGameResult.Fail(
                    UserGameError.Validation,
                    "UserId and GameId are required.");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return UserGameResult.Fail(
                    UserGameError.NotFound,
                    "User not found.");
            }

            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game is null)
            {
                return UserGameResult.Fail(
                    UserGameError.NotFound,
                    "Game not found.");
            }

            var exists = await _userRepository.ExistsGameAsync(userId, gameId);
            if (exists)
            {
                return UserGameResult.Fail(
                    UserGameError.Conflict,
                    "User already owns this game.");
            }

            await _userRepository.AddGameAsync(new UserGame(user.Id, game.Id));
            await _userRepository.SaveChangesAsync();

            return UserGameResult.Ok();
        }
    }
}
