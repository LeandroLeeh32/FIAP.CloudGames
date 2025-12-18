using FIAP.CloudGames.Application.UseCases.ProcessGame;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FIAP.CloudGames.Tests.Application.UseCases.ProcessGame
{
    public class ProcessGameUseCaseTests
    {
        private readonly Mock<ILogger<ProcessGameUseCase>> _loggerMock;
        private readonly ProcessGameUseCase _useCase;

        public ProcessGameUseCaseTests()
        {
            _loggerMock = new Mock<ILogger<ProcessGameUseCase>>();
            _useCase = new ProcessGameUseCase(_loggerMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnSuccess_WhenGameNameIsValid()
        {
            // Arrange
            var request = new ProcessGameRequest
            {
                GameName = "FIFA 24"
            };

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Jogo 'FIFA 24' processado com sucesso.", result.Message);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFailure_WhenGameNameIsEmpty()
        {
            // Arrange
            var request = new ProcessGameRequest
            {
                GameName = ""
            };

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nome do jogo é obrigatório.", result.Message);
        }
    }


}
