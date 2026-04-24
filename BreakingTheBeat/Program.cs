using MediatR;
using MusicCatalog.Features.Language.GetLanguages;
using MusicCatalog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.Features.Language.Interfaces;
using MusicCatalog.Infrastructure.Repositories;
using MusicCatalog.Features.Genre;
using MusicCatalog.Features.Genre.Interfaces;
using MusicCatalog.Features.Track;
using MusicCatalog.Features.Track.Create;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Features.Podcasts.Create;
using PodcastCatalog.Features.Podcasts.Get;
using PodcastCatalog.Features.Podcasts.GetAll;
using PodcastCatalog.Features.Podcasts.Update;
using PodcastCatalog.Infrastructure;
using PodcastCatalog.Infrastructure.Data;
using PodcastCatalog.Infrastructure.Repositories;
using PodcastCatalog.Shared.Infrastructure.Repositories;
using SharedKernel;
using SharedKernel.Interfaces.Base;

namespace Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Connection strings
            var musicCatalogConnectionString = builder.Configuration.GetConnectionString("MusicCatDb");

            // 2. DbContext
            builder.Services.AddDbContext<MusicCatalogDbContext>(options => options.UseNpgsql(musicCatalogConnectionString));

            // 3. Repositories
            builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
            builder.Services.AddScoped<IGenreRepository, GenreRepository>();
            builder.Services.AddScoped<ITrackRepository, TrackRepository>();

            //4. services


            //5. Command/query handlers are registered by MediatR
            builder.Services.AddMediatR(cfg =>
               cfg.RegisterServicesFromAssembly(typeof(GetLanguagesQuery).Assembly));
            builder.Services.AddMediatR(cfg =>
             cfg.RegisterServicesFromAssembly(typeof(GetGenresQuery).Assembly));
            builder.Services.AddMediatR(cfg =>
             cfg.RegisterServicesFromAssembly(typeof(CreateTrackCommand).Assembly));
            // 1. Connection strings
            var podcastCatalogConnectionString = builder.Configuration.GetConnectionString("PodcastCatalog")
                ?? throw new InvalidOperationException("Connection string 'PodcastCatalog' not found.");

            // 2. DbContext
            builder.Services.AddDbContext<PodcastCatalogDbContext>(options =>
                options.UseNpgsql(podcastCatalogConnectionString));

            // 3.1.  Podcast Repositories
            builder.Services.AddScoped<ICrudRepository<Podcast>, PodcastRepository>();
            builder.Services.AddScoped<ICrudRepository<Language>, LanguageRepository>();

            // 4.1. Podcast services
            builder.Services.AddValidatorsFromAssemblyContaining<CreatePodcastValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdatePodcastValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetPodcastValidator>();


            //6. Controllers
            // 5.1. Podcast command/query handlers are registered by MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePodcastHandler).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdatePodcastHandler).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPodcastHandler).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllPodcastsHandler).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPodcastHandler).Assembly));


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
