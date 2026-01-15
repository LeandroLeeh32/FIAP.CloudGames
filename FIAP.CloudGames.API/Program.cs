using System.Reflection;
using FIAP.CloudGames.API.Middlewares;
using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Application.Interfaces.Security;
using FIAP.CloudGames.Application.Interfaces.Services;
using FIAP.CloudGames.Application.Repositories;
using FIAP.CloudGames.Application.UseCases.Authentication;
using FIAP.CloudGames.Application.UseCases.Games;
using FIAP.CloudGames.Application.UseCases.PromotionGames;
using FIAP.CloudGames.Application.UseCases.Promotions;
using FIAP.CloudGames.Application.UseCases.UserGames;
using FIAP.CloudGames.Application.UseCases.Users;
using FIAP.CloudGames.Infrastructure.Persistence.Context;
using FIAP.CloudGames.Infrastructure.Persistence.Repositories;
using FIAP.CloudGames.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

var dbPath = Path.Combine(builder.Environment.ContentRootPath, "fiap.cloudgames.db");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));

Console.WriteLine($"[DB] ConnectionString: {connectionString}");

// UseCases
builder.Services.AddScoped<LoginUserUseCase>();
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<GetUsersUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();
builder.Services.AddScoped<GetUserByIdUseCase>();
builder.Services.AddScoped<GetGamesUseCase>();
builder.Services.AddScoped<GetGameByIdUseCase>();
builder.Services.AddScoped<CreateGameUseCase>();
builder.Services.AddScoped<UpdateGameUseCase>();
builder.Services.AddScoped<DeleteGameUseCase>();
builder.Services.AddScoped<GetPromotionsUseCase>();
builder.Services.AddScoped<GetPromotionByIdUseCase>();
builder.Services.AddScoped<CreatePromotionUseCase>();
builder.Services.AddScoped<UpdatePromotionUseCase>();
builder.Services.AddScoped<DeletePromotionUseCase>();
builder.Services.AddScoped<AddGameToPromotionUseCase>();
builder.Services.AddScoped<AddGameToUserUseCase>();

// Dependency Injection (Infrastructure)
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserGameRepository, UserGameRepository>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();

// Controllers & Swagger
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

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// JWT Authentication
builder.Services.AddJwtAuthentication(
    "FIAP_CLOUD_GAMES_SUPER_SECRET_KEY_123456_123456");

// Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
