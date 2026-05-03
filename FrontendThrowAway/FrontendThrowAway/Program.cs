using FrontendThrowAway.Auth;
using FrontendThrowAway.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

namespace FrontendThrowAway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // API base URL
            var apiBaseUrl = builder.Configuration["API_BASE_URL"] ?? "https://localhost:7194/";

            // Blazor / Razor components
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            // MudBlazor
            builder.Services.AddMudServices();

            // Authentication / authorization for Blazor UI
            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();

            // Custom auth state provider
            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
                sp.GetRequiredService<CustomAuthStateProvider>());

            // HttpClient for API calls
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(apiBaseUrl)
            });

            Console.WriteLine($"API Base URL: {apiBaseUrl}");

            var app = builder.Build();

            // Error handling / dev tools
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Middleware
            app.UseHttpsRedirection();
            app.UseAntiforgery();

            // Static files and Blazor app
            app.MapStaticAssets();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }
    }
}
