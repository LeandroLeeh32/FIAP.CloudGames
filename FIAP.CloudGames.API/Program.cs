using FIAP.CloudGames.Application.Repositories;
using FIAP.CloudGames.Application.UseCases.Games;
using FIAP.CloudGames.Infrastructure.Persistence;
using FIAP.CloudGames.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// ?? Conecta NLog ao pipeline de logging
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// ?? Dependency Injection
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGetGamesUseCase, GetGamesUseCase>();
builder.Services.AddScoped<IGetGameByIdUseCase, GetGameByIdUseCase>();
builder.Services.AddScoped<ICreateGameUseCase, CreateGameUseCase>();
builder.Services.AddScoped<IUpdateGameUseCase, UpdateGameUseCase>();
builder.Services.AddScoped<IDeleteGameUseCase, DeleteGameUseCase>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));


// ?? Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ?? Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
