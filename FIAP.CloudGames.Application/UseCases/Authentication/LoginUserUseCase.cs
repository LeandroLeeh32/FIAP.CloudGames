using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Application.Interfaces.Security;
using FIAP.CloudGames.Application.Interfaces.Services;
using FIAP.CloudGames.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Authentication
{
    public class LoginUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly ITokenService _authService;
        private readonly ILogger<LoginUserUseCase> _logger;

        public LoginUserUseCase(
            IUserRepository userRepository,
            IPasswordHashService passwordHashService,
            ITokenService authService,
            ILogger<LoginUserUseCase> logger)
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
            _authService = authService;
            _logger = logger;
        }

        public async Task<string> ExecuteAsync(string email, string password)
        {
            _logger.LogInformation(
                "[App][LoginUserUseCase] Iniciando fluxo de autenticação para {Email}",
                email);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning(
                    "[App][LoginUserUseCase] Tentativa de login com dados inválidos");
                throw new ArgumentException("[App][LoginUserUseCase] E-mail e senha são obrigatórios");
            }

            //Buscar usuário real no banco
            var user = await _userRepository.GetByEmailAsync(email);

            if (user is null)
            {
                _logger.LogWarning(
                    "[App][LoginUserUseCase] Usuário não encontrado para o email {Email}",
                    email);

                throw new Exception("[App][LoginUserUseCase] Credenciais inválidas");
            }

            //Validar senha
            var passwordValid = _passwordHashService.Verify(password, user.PasswordHash);

            if (!passwordValid)
            {
                _logger.LogWarning(
                    "[App][LoginUserUseCase] Senha inválida para o email {Email}",
                    email);

                throw new Exception("[App][LoginUserUseCase] Credenciais inválidas");
            }

            _logger.LogInformation(
                "[App][LoginUserUseCase] Autenticação realizada com sucesso para {Email}",
                email);

            // 3️⃣ Gerar JWT com usuário REAL
            return _authService.GenerateToken(user);
        }
    }
}
