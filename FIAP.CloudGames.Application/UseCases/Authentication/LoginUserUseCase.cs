using FIAP.CloudGames.Application.Interfaces;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.CloudGames.Application.UseCases.Authentication
{
    public class LoginUserUseCase
    {
        private readonly ITokenService _authService;
        private readonly ILogger<LoginUserUseCase> _logger;
        public LoginUserUseCase(ITokenService authService, ILogger<LoginUserUseCase> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public string Execute(string email)
        {

            _logger.LogInformation("[App][LoginUserUseCase] Iniciando fluxo de autenticação");

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("[App][LoginUserUseCase] Tentativa de login com e-mail vazio");
                throw new ArgumentException("E-mail é obrigatório");
            }

            var role = email.Contains("admin")? UserRole.Admin: UserRole.User;

            _logger.LogInformation("[App][LoginUserUseCase] Perfil atribuído ao usuário: {Perfil}",role);

            var user = new User("Mock User", email, role);

            return _authService.GenerateToken(user);

        }
    }

}
