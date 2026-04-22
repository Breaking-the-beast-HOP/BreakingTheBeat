using FluentValidation;
using MediatR;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Features.Podcasts.Create;
using PodcastCatalog.Features.Podcasts.Get;
using PodcastCatalog.Features.Podcasts.Update;
using PodcastCatalog.Shared.Infrastructure.Repositories;
using SharedKernel;
using SharedKernel.Interfaces.Base;

namespace BreakingTheBeat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Connection strings
            var podcastCatalogConnectionString = builder.Configuration.GetConnectionString("PodcastCatalog")
                ?? throw new InvalidOperationException("Connection string 'PodcastCatalog' not found.");

            // 2. DbContext

            // 3. Repositories
            builder.Services.AddScoped<ICrudRepository<Podcast>, PodcastRepository>();
            builder.Services.AddScoped<PodcastRepository, PodcastRepository>();

            // 4. Services
            builder.Services.AddValidatorsFromAssemblyContaining<CreatePodcastValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdatePodcastValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetPodcastValidator>();

            // 5. Command/query handlers are registered by MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePodcastHandler).Assembly));

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

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
        }
    }
}
