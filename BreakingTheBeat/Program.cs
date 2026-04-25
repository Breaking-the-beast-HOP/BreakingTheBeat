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
using MusicCatalog.Features.Contributor.Create;
using MusicCatalog.Features.Contributor;

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
            builder.Services.AddScoped<IContributorRepository, ContributorRepository>();

            //4. services


            //5. Command/query handlers are registered by MediatR
            builder.Services.AddMediatR(cfg =>
               cfg.RegisterServicesFromAssembly(typeof(GetLanguagesQuery).Assembly));
            builder.Services.AddMediatR(cfg =>
             cfg.RegisterServicesFromAssembly(typeof(GetGenresQuery).Assembly));
            builder.Services.AddMediatR(cfg =>
             cfg.RegisterServicesFromAssembly(typeof(CreateTrackCommand).Assembly));

            builder.Services.AddMediatR(cfg =>
          cfg.RegisterServicesFromAssembly(typeof(CreateContributorCommand).Assembly));

            //6. Controllers
            builder.Services.AddControllers();

            //7. Swagger/OpenAPI
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            //8. CORS

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
