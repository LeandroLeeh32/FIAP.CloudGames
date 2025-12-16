using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// ?? Conecta NLog ao pipeline de logging
builder.Logging.ClearProviders();
builder.Host.UseNLog();

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
