using Microsoft.EntityFrameworkCore;
using Monitoring;
using Monitoring.Components;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("LoggingDb");

if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("Connection string 'LoggingDb' not found in secrets");

builder.Services.AddDbContextFactory<LoggingDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});


builder.Services.AddTransient(options =>
    new NpgsqlConnection(connectionString));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
