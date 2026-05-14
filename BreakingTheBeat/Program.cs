using System.Security.Claims;
using System.Text;
using FluentValidation;
using Gateway.Logging.Behaviors;
using Gateway.Logging.Data;
using Gateway.Logging.Entities;
using Gateway.Logging.Middleware;
using Gateway.Logging.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicCatalog.Features.Contributor;
using MusicCatalog.Features.Contributor.Create;
using MusicCatalog.Features.ContributorRole;
using MusicCatalog.Features.ContributorRole.GetAll;
using MusicCatalog.Features.Genre;
using MusicCatalog.Features.Genre.Interfaces;
using MusicCatalog.Features.Language.GetLanguages;
using MusicCatalog.Features.Language.Interfaces;
using MusicCatalog.Features.Release;
using MusicCatalog.Features.Release.Create;
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
using PodcastCatalog.Infrastructure.Repositories;
using SharedKernel;
using SharedKernel.Interfaces.Base;
using UserManagement.Domain.Entities;
using UserManagement.Features.User.GetAll;
using UserManagement.Features.User.Register;
using UserManagement.Infrastructure.Auth;
using UserManagement.Infrastructure.Persistence;

namespace Gateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // 1. Connection strings
            var musicCatalogConnectionString = builder.Configuration.GetConnectionString("MusicCatDb");

            var podcastCatalogConnectionString = builder.Configuration.GetConnectionString("PodcastCatDb")
                ?? throw new InvalidOperationException("Connection string 'PodcastCatDb' not found.");

            var userManagementConnectionString = builder.Configuration.GetConnectionString("UserManagementDb")
                ?? throw new InvalidOperationException("Connection string 'UserManagementDb' not found.");

            var LoggingDbConnectionString = builder.Configuration.GetConnectionString("LoggingDb");

            // 2. DbContexts
            builder.Services.AddDbContext<MusicCatalogDbContext>(options =>
                options.UseNpgsql(musicCatalogConnectionString));

            builder.Services.AddDbContext<PodcastCatalogDbContext>(options =>
                options.UseNpgsql(podcastCatalogConnectionString));

            builder.Services.AddDbContext<UserManagementDbContext>(options =>
                options.UseNpgsql(userManagementConnectionString));

            builder.Services.AddDbContext<LoggingDbContext>(options =>
                options.UseNpgsql(LoggingDbConnectionString));

            // 3. Identity
            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<UserManagementDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<JwtService>();

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),

                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.Name
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddScoped<ILoggingService, LoggingService>();

            // 4. Repositories (music)
            builder.Services.AddScoped<ILanguageRepository, MusicCatalog.Infrastructure.Repositories.LanguageRepository>();
            builder.Services.AddScoped<IGenreRepository, GenreRepository>();
            builder.Services.AddScoped<ITrackRepository, TrackRepository>();
            builder.Services.AddScoped<IContributorRepository, MusicCatalog.Infrastructure.Repositories.ContributorRepository>();
            builder.Services.AddScoped<IContributorRoleRepository, ContributionRoleRepository>();
            builder.Services.AddScoped<IReleaseRepository, ReleaseRepository>();

            // 5. Repositories (podcast)
            builder.Services.AddScoped<ICrudRepository<Podcast>, PodcastRepository>();
            builder.Services.AddScoped<ICrudRepository<Language>, PodcastCatalog.Infrastructure.Repositories.LanguageRepository>();
            builder.Services.AddScoped<IReadRepository<Category>, CategoryRepository>();
            builder.Services.AddScoped<IReadRepository<ContributorRole>, ContributorRoleRepository>();
            builder.Services.AddScoped<ICrudRepository<Contributor>, PodcastCatalog.Infrastructure.Repositories.ContributorRepository>();

            // 6. Validators (podcast)
            builder.Services.AddValidatorsFromAssemblyContaining<CreatePodcastValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdatePodcastValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetPodcastValidator>();

            // 7. MediatR
            builder.Services.AddMediatR(cfg =>
            {
                // Music
                cfg.RegisterServicesFromAssembly(typeof(GetLanguagesQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetGenresQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(CreateTrackCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(CreateContributorCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(CreateReleaseCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetContributorRolesQuery).Assembly);

                // Podcast
                cfg.RegisterServicesFromAssembly(typeof(CreatePodcastHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(UpdatePodcastHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetPodcastHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetAllPodcastsHandler).Assembly);

                // UserManagement
                cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetUsersQuery).Assembly);
            });

            // Logging MediatR
            builder.Services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(LoggingBehavior<,>));
            builder.Services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ExceptionHandlingBehavior<,>));

            // 8. Controllers
            builder.Services.AddControllers();

            // 9. Swagger/OpenAPI
            builder.Services.AddOpenApi();

            var app = builder.Build();


            // Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Logging
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();

            // logging
            await LogStartupAsync(app.Services, $"Application started in {app.Environment.EnvironmentName}");

            // Logging if Connectionstrings are loaded correctly
            if (musicCatalogConnectionString != null)
                await LogStartupAsync(app.Services,
                    $"musicCatalogConnectionString: " +
                    $"{SanitizeConnectionString(musicCatalogConnectionString)}");
            else await LogStartupErrorAsync(app.Services,
                "musicCatalogDb not loaded");

            if (podcastCatalogConnectionString != null)
                await LogStartupAsync(app.Services,
                    $"podcastCatalogConnectionString: " +
                    $"{SanitizeConnectionString(podcastCatalogConnectionString)}");
            else await LogStartupErrorAsync(app.Services,
                "podcastCatalogDb not loaded");

            if (userManagementConnectionString != null)
                await LogStartupAsync(app.Services,
                    $"{SanitizeConnectionString(userManagementConnectionString)}");
            else await LogStartupErrorAsync(app.Services, "userManagementDb not loaded");
        }

        private static async Task LogStartupAsync(
            IServiceProvider services,
            string message)
        {
            using var scope = services.CreateScope();

            var loggingService = scope.ServiceProvider.GetService<ILoggingService>();

            await loggingService.LogAsync(new LogEntity
            {
                RequestId = "SYSTEM",
                LogLevel = Logging.Entities.ApplicationLogLevel.Info,
                Message = message,
                RequestName = "ApplicationStartup"
            });
        }

        private static async Task LogStartupErrorAsync(
            IServiceProvider services,
            string message)
        {
            using var scope = services.CreateScope();
            var loggingService = scope.ServiceProvider.GetService<ILoggingService>();

            await loggingService.LogAsync(new LogEntity
            {
                RequestId = "SYSTEM",
                LogLevel = Logging.Entities.ApplicationLogLevel.Warning,
                Message = message,
                RequestName = "ApplicationStartup"
            });
        }
        private static async Task LogStartupErrorAsync(
            IServiceProvider services,
            string message, Exception ex)
        {
            using var scope = services.CreateScope();

            var loggingService = scope.ServiceProvider.GetService<ILoggingService>();

            await loggingService.LogAsync(new LogEntity
            {
                RequestId = "SYSTEM",
                LogLevel = Logging.Entities.ApplicationLogLevel.Warning,
                Message = message,
                RequestName = "ApplicationStartup",
                Exception = ex.ToString()
            });
        }

        private static string SanitizeConnectionString(string connectionString)
        {
            var parts = connectionString.Split(';');

            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].StartsWith("Password=", StringComparison.OrdinalIgnoreCase))
                {
                    parts[i] = "Password=****";
                }
            }

            return string.Join(';', parts);
        }
    }
}