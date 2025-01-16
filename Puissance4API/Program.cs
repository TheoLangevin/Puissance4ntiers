using Microsoft.EntityFrameworkCore;
using Puissance4Model.Data;
var builder = WebApplication.CreateBuilder(args);

// Ajouter le service du contexte EF Core
builder.Services.AddDbContext<ApplicationDbContext>();

// Ajouter les services pour les contrôleurs
builder.Services.AddControllers();

// Activer Swagger pour la documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurer l'application
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();