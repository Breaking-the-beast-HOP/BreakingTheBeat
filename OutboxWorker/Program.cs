using Microsoft.EntityFrameworkCore;
using MusicCatalogContext.Infrastructure;
using OutboxWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<MusicCatalogDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MusicCatalogDb");
    options.UseNpgsql(connectionString);
});

builder.Services.AddHttpClient("QcApi", client =>
{
    var baseUrl = builder.Configuration["QcApi:BaseUrl"];
    var workerKey = builder.Configuration["WorkerAuth:Key"];

    client.BaseAddress = new Uri(baseUrl!);
    client.DefaultRequestHeaders.Add("X-Worker-Key", workerKey);
});
builder.Services.AddScoped<OutboxProcessor>();

var workerEnabled = builder.Configuration.GetValue<bool>("OutboxWorker:Enabled");

if (workerEnabled)
{
    builder.Services.AddHostedService<Worker>();
}

var host = builder.Build();
host.Run();