using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.Features.Genre;
using MusicCatalog.Features.Genre.Interfaces;
using MusicCatalog.Features.Language.GetLanguages;
using MusicCatalog.Features.Language.Interfaces;
using MusicCatalog.Features.Track;
using MusicCatalog.Features.Track.Create;
using MusicCatalog.Infrastructure;
using MusicCatalog.Infrastructure.Repositories;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Features.Podcasts.Create;
using PodcastCatalog.Features.Podcasts.Get;
using PodcastCatalog.Features.Podcasts.GetAll;
using PodcastCatalog.Features.Podcasts.Update;
using PodcastCatalog.Infrastructure;
using PodcastCatalog.Infrastructure.Data;
using PodcastCatalog.Shared.Infrastructure.Repositories;
using SharedKernel;

namespace Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Connection strings
            var musicCatalogConnectionString = builder.Configuration.GetConnectionString("MusicCatDb");
            var podcastCatalogConnectionString = builder.Configuration.GetConnectionString("PodcastCatDb") ?? throw new InvalidOperationException("Connection string 'PodcastCatalog' not found.");

            // 2. DbContext
            Console.WriteLine($"Music Catalog Connection String: {musicCatalogConnectionString}");
            builder.Services.AddDbContext<MusicCatalogDbContext>(options => options.UseNpgsql(musicCatalogConnectionString));
            Console.WriteLine($"Podcast Catalog Connection String: {podcastCatalogConnectionString}");
            builder.Services.AddDbContext<PodcastCatalogDbContext>(options => options.UseNpgsql(podcastCatalogConnectionString));

            // 3.1 Repositories (music)
            builder.Services.AddScoped<ILanguageRepository, MusicCatalog.Infrastructure.Repositories.LanguageRepository>();
            builder.Services.AddScoped<IGenreRepository, GenreRepository>();
            builder.Services.AddScoped<ITrackRepository, TrackRepository>();

            // 3.2. Repositories (podcast)
            builder.Services.AddScoped<ICrudRepository<Podcast>, PodcastRepository>();
            builder.Services.AddScoped<ICrudRepository<Language>, PodcastCatalog.Infrastructure.Repositories.LanguageRepository>();

            //4.1. Music services

            // 4.2. Podcast services
            builder.Services.AddValidatorsFromAssemblyContaining<CreatePodcastValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdatePodcastValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetPodcastValidator>();

            //5.1. Command/query handlers are registered by MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetLanguagesQuery).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetGenresQuery).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTrackCommand).Assembly));

            // 5.2. Podcast command/query handlers are registered by MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePodcastHandler).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdatePodcastHandler).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPodcastHandler).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllPodcastsHandler).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPodcastHandler).Assembly));

         
            //6. Controllers
            builder.Services.AddControllers();

            //7. Swagger/OpenAPI
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            //8. CORS


            var app = builder.Build();

            // Seed the database with initial data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<PodcastCatalogDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

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
