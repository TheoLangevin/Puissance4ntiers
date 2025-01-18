using Puissance4Web.Components;
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Ajouter Blazored.LocalStorage au conteneur de services
builder.Services.AddBlazoredLocalStorage();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5263/") });
builder.Services.AddScoped<AuthService>();

// Ajouter une politique CORS (si nécessaire)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN"; // Définit le nom de l'en-tête pour transmettre le token
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Page détaillée pour les erreurs en mode développement
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts(); // Sécurise les connexions HTTPS en production
}

// Redirection HTTP vers HTTPS
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// Ajouter CORS (si nécessaire)
app.UseCors();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();