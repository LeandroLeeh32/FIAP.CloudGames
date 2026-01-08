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
        private readonly IAuthService _authService;
        private readonly ILogger<LoginUserUseCase> _logger;
        public LoginUserUseCase(IAuthService authService, ILogger<LoginUserUseCase> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public string Execute(string email)
        {
            try
            {
                _logger.LogInformation(
                "Iniciando login para o e-mail {Email}", email);

                if (string.IsNullOrWhiteSpace(email))
                {
                    _logger.LogWarning("Tentativa de login com e-mail vazio");
                    throw new ArgumentException("E-mail é obrigatório");
                }

                var role = email.Contains("admin")
                    ? UserRole.Admin
                    : UserRole.User;

                _logger.LogInformation(
                    "Perfil atribuído ao usuário {Email}: {Role}",
                    email, role);

                var user = new User("Mock User", email, role);

                _logger.LogInformation(
                    "Token JWT gerado com sucesso para {Email}", email);

                return _authService.GenerateToken(user);
            }
            catch (Exception ex)
            {

                _logger.LogError(
                    ex.Message);
                throw;
            }
            
        }
    }

}
