using Frontend.Auth;
using Frontend.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("=== Application starting ===");

// API base URL
var apiBaseUrl = builder.Configuration["API_BASE_URL"]
                 ?? "https://localhost:7194/";

Console.WriteLine($"Frontend Base URL: {apiBaseUrl}");
// Razor Components (Blazor Server)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// MudBlazor
builder.Services.AddMudServices();

// Auth (Blazor UI)
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// Custom auth provider
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthStateProvider>());

// HttpClient (API calls)
builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl)
});

try
{
    var app = builder.Build();

    Console.WriteLine("=== App built successfully ===");
    // Error handling
    if (!app.Environment.IsDevelopment())
    {
        Console.WriteLine("Running Production");

        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }
    else
    {
        Console.WriteLine("Running Development");

        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();

    app.UseAntiforgery();

    // Static assets + Blazor app
    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    Console.WriteLine("=== Starting Kestrel ===");
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        Console.WriteLine("=== Application Started ===");
    });

    app.Run();
}
catch (Exception ex)
{
    // Log the exception (you can use a logging framework here)
    Console.Error.WriteLine($"An error occurred: {ex.Message}");
    Console.Error.WriteLine(ex.StackTrace);
}
