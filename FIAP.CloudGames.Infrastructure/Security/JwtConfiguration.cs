using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FIAP.CloudGames.Infrastructure.Security;

public static class JwtConfiguration
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        string secretKey)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "FIAP.CloudGames",
                    ValidAudience = "FIAP.CloudGames",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        var logger = context.HttpContext
                            .RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("FIAP.CloudGames.API.Auth");

                        logger.LogWarning("[API][Auth] Usuário não autenticado ao acessar {Caminho}",context.HttpContext.Request.Path);

                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        return context.Response.WriteAsJsonAsync(new
                        {
                            error = "Usuário não autenticado ou token inválido"
                        });
                    },

                    OnForbidden = context =>
                    {
                        var logger = context.HttpContext
                            .RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("Auth");

                        logger.LogWarning("[API][Auth] Usuário autenticado sem permissão para acessar {Caminho}",context.HttpContext.Request.Path);

                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        return context.Response.WriteAsJsonAsync(new
                        {
                            error = "Usuário autenticado, porém sem permissão para acessar este recurso"
                        });
                    }
                };

            });

        services.AddAuthorization();
        return services;
    }
}
