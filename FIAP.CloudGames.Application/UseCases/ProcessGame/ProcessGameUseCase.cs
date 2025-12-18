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
            _logger.LogInformation("Iniciando processamento do jogo. Nome: {GameName}",request.GameName);

            if (string.IsNullOrWhiteSpace(request.GameName))
            {
                _logger.LogWarning("Nome do jogo não informado.");

                return Task.FromResult(new ProcessGameResponse
                {
                    Success = false,
                    Message = "Nome do jogo é obrigatório."
                });
            }

            _logger.LogInformation(
                "Processamento do jogo {GameName} realizado com sucesso.",
                request.GameName
            );

            return Task.FromResult(new ProcessGameResponse
            {
                Success = true,
                Message = $"Jogo '{request.GameName}' processado com sucesso."
            });

        }
    }
}
