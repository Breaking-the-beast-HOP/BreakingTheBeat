using Microsoft.Extensions.Options;
using PodcastCatalog.Endpoints;
using FluentValidation;
using PodcastCatalog.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddEndpoints();

builder.Services.AddDbContext<PodcastCatalogDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PodcastCatalogConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

