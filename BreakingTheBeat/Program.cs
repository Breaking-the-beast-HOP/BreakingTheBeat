using System.Security.Claims;
using System.Text;
using FluentValidation;
using Gateway.Logging.Behaviors;
using Gateway.Logging.Entities;
using Gateway.Logging.Middleware;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
using PodcastCatalog.Infrastructure.Data;
using PodcastCatalog.Infrastructure.Repositories;
using Serilog;
using Serilog.Sinks.PostgreSQL;
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

            Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.PostgreSQL(
                    connectionString: builder.Configuration.GetConnectionString("LoggingDb"),
                    tableName: "GatewayLogs",
                    needAutoCreateTable: true,
                    columnOptions: new Dictionary<string, ColumnWriterBase>
                    {
                        ["message"] = new RenderedMessageColumnWriter(),
                        ["message_template"] = new MessageTemplateColumnWriter(),
                        ["level"] = new LevelColumnWriter(),
                        ["timestamp"] = new TimestampColumnWriter(),
                        ["exception"] = new ExceptionColumnWriter(),
                        ["log_event"] = new LogEventSerializedColumnWriter(),
                        ["properties"] = new PropertiesColumnWriter()
                    })
                .CreateLogger();

            builder.Host.UseSerilog();
            builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);
            builder.Services.AddHttpContextAccessor();

            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];
            var jwtKey = builder.Configuration["Jwt:Key"];

            Log.Information(
                "JWT configuration loaded | Issuer: {Issuer} | Audience: {Audience} | KeyConfigured: {KeyConfigured} | KeyLength: {KeyLength}",
                jwtIssuer,
                jwtAudience,
                !string.IsNullOrWhiteSpace(jwtKey),
                jwtKey?.Length ?? 0);

            // 1. Connection strings
            var musicCatalogConnectionString = builder.Configuration.GetConnectionString("MusicCatDb");

            var podcastCatalogConnectionString = builder.Configuration.GetConnectionString("PodcastCatDb")
                ?? throw new InvalidOperationException("Connection string 'PodcastCatDb' not found.");

            var userManagementConnectionString = builder.Configuration.GetConnectionString("UserManagementDb")
                ?? throw new InvalidOperationException("Connection string 'UserManagementDb' not found.");

            // 2. DbContexts
            builder.Services.AddDbContext<MusicCatalogDbContext>(options =>
                options.UseNpgsql(musicCatalogConnectionString));

            builder.Services.AddDbContext<PodcastCatalogDbContext>(options =>
                options.UseNpgsql(podcastCatalogConnectionString));

            builder.Services.AddDbContext<UserManagementDbContext>(options =>
                options.UseNpgsql(userManagementConnectionString));


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

            app.UseSerilogRequestLogging();

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

            // Startup logs
            Log.Information(
                "Application started in {Environment}",
                app.Environment.EnvironmentName);

            if (musicCatalogConnectionString is null)
            {
                Log.Warning("MusicCatalog connection string not loaded");
            }
            else
            {
                Log.Information(
                    "MusicCatalog connection loaded: {ConnectionString}",
                    SanitizeConnectionString(musicCatalogConnectionString!));
            }


            if (podcastCatalogConnectionString is null)
            {
                Log.Warning("PodcastCatalog connection string not loaded");
            }
            else
            {
                Log.Information(
                    "PodcastCatalog connection loaded: {ConnectionString}",
                    SanitizeConnectionString(podcastCatalogConnectionString));
            }

            if (userManagementConnectionString is null)
            {
                Log.Warning("UserManagement connection string not loaded");
            }
            else
            {
                Log.Information(
                    "UserManagement connection loaded: {ConnectionString}",
                    SanitizeConnectionString(userManagementConnectionString));
            }

            try
            {
                Log.Information("Starting web application");

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }


        }

        // logging


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