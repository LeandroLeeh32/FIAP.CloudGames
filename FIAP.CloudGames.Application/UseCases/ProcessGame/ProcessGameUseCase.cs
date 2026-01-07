using FIAP.CloudGames.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.CloudGames.Application.UseCases.ProcessGame

{

    public class ProcessGameUseCase : IProcessGameUseCase
    {

        private readonly ILogger<ProcessGameUseCase> _logger;

        public ProcessGameUseCase(ILogger<ProcessGameUseCase> logger)
        {
            _logger = logger;
        }

        public Task<ProcessGameResponse> ExecuteAsync(ProcessGameRequest request)
        {
            _logger.LogInformation("Iniciando processamento do jogo. Nome: {GameName}", request.GameName);

            try
            {
                var game = new Game(request.GameName);
                _logger.LogInformation("Processamento do jogo {GameName} realizado com sucesso.", game.Name);

                return Task.FromResult(new ProcessGameResponse
                {
                    Success = true,
                    Message = $"Jogo '{game.Name}' processado com sucesso."
                });

            }
            catch (Exception ex)
            {
                _logger.LogWarning("Falha ao processar o jogo por nome inválido.");

                return Task.FromResult(new ProcessGameResponse
                {
                    Success = false,
                    Message = "Nome do jogo é obrigatório."
                });
            }
        }
    }
}
