using Microsoft.EntityFrameworkCore;
using TurneroMedico.Server.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// DbContext InMemory para desarrollo
builder.Services.AddDbContext<TurneroContext>(opt =>
{
    opt.UseInMemoryDatabase("TurneroDb");
});

var app = builder.Build();

// Seed de datos iniciales
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TurneroContext>();
    SeedData.Initialize(db);
}

app.MapDefaultEndpoints();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
