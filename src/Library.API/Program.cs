using DotNetEnv;
using Library.Infrastructure;
using Library.Application;

var currentDir = Directory.GetCurrentDirectory();
var envPath = Path.Combine(currentDir, ".env");
if(!File.Exists(envPath))
{
    envPath = Path.Combine(currentDir, "..", "..", ".env");
}
if (File.Exists(envPath))
{
    Env.Load(envPath);
    Console.WriteLine($".env file loaded from: {envPath}");
    Console.WriteLine(envPath.ToString());
}
else
{
    Console.WriteLine(".env file not found.");
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
// Global exception handling middleware (normalize error responses JSON en español)
app.UseMiddleware<Library.API.Middleware.ExceptionHandlingMiddleware>();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
