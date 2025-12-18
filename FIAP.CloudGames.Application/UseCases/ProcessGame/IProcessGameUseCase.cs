using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.CloudGames.Application.UseCases.ProcessGame
{
    public interface IProcessGameUseCase
    {
        Task<ProcessGameResponse> ExecuteAsync(ProcessGameRequest request);
    }
}
