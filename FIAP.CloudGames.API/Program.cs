using FIAP.CloudGames.Application.UseCases.Authentication;
using FIAP.CloudGames.Application.UseCases.ProcessGame;
using FIAP.CloudGames.Application.Interfaces;
using FIAP.CloudGames.Infrastructure.Security;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Text;
using FIAP.CloudGames.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Conecta NLog ao pipeline de logging
// 🔹 Logging (NLog)
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// 🔹 Dependency Injection (Application)
builder.Services.AddScoped<IProcessGameUseCase, ProcessGameUseCase>();
builder.Services.AddScoped<LoginUserUseCase>();

// 🔹 Dependency Injection (Infrastructure)
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// 🔹 Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Informe o token JWT no formato: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 🔹 JWT Authentication
builder.Services.AddJwtAuthentication(
    "FIAP_CLOUD_GAMES_SUPER_SECRET_KEY_123456_123456");

// 🔹 Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware de logging
app.UseMiddleware<RequestLoggingMiddleware>();

// ?? Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
