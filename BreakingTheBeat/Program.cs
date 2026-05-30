using System.Security.Claims;
using System.Text;
using FluentValidation;
using Gateway.Infrastructure.Auth;
using Gateway.Logging.Behaviors;
using Gateway.Logging.Middleware;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicCatalog.Features.Label.Create;
using MusicCatalogContext.Features;
using MusicCatalogContext.Features.Contributor;
using MusicCatalogContext.Features.Contributor.Create;
using MusicCatalogContext.Features.ContributorRole;
using MusicCatalogContext.Features.ContributorRole.GetAll;
using MusicCatalogContext.Features.Countries.Get;
using MusicCatalogContext.Features.Genre;
using MusicCatalogContext.Features.Genre.Interfaces;
using MusicCatalogContext.Features.Language.GetLanguages;
using MusicCatalogContext.Features.Language.Interfaces;
using MusicCatalogContext.Features.Release;
using MusicCatalogContext.Features.Release.Create;
using MusicCatalogContext.Features.Track;
using MusicCatalogContext.Features.Track.Create;
using MusicCatalogContext.Infrastructure;
using MusicCatalogContext.Infrastructure.Repositories;
using MusicCatalogContext.Infrastructure.UnitOfWorks;
using MusicQualityControl.Features;
using MusicQualityControl.Features.GetQcQueue;
using MusicQualityControl.Features.SubmitReleaseToQc;
using MusicQualityControl.Infrastructure;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Features.Podcasts.Interfaces;
using PodcastCatalog.Features.Podcasts.PodcastCore.Create;
using PodcastCatalog.Features.Podcasts.PodcastCore.Get;
using PodcastCatalog.Features.Podcasts.PodcastCore.GetAll;
using PodcastCatalog.Features.Podcasts.PodcastCore.Update;
using PodcastCatalog.Infrastructure;
using PodcastCatalog.Infrastructure.Repositories;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Sinks.PostgreSQL;
using SharedKernel;
using SharedKernel.Interfaces;
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

            builder.Services.AddHttpContextAccessor();

            // Serilog
#pragma warning disable CA1305

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()

                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)

                .Enrich.FromLogContext()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.WithSpan()

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

#pragma warning restore CA1305

            builder.Host.UseSerilog();

            var musicCatalogConnectionString = builder.Configuration.GetConnectionString("MusicCatalogDb");
            var podcastCatalogConnectionString = builder.Configuration.GetConnectionString("PodcastCatalogDb");
            var userManagementConnectionString = builder.Configuration.GetConnectionString("UserManagementDb");
            var musicQualityControlConnectionString = builder.Configuration.GetConnectionString("MusicQualityControlDb");

            builder.Services.AddScoped<PostgresRoleService>();
            builder.Services.AddScoped<PostgresRoleInterceptor>();

            // 2. DbContexts
            builder.Services.AddDbContext<MusicCatalogDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<PostgresRoleInterceptor>();

                var connectionString =
                    builder.Configuration.GetConnectionString("MusicCatalogDb")
                    ?? throw new InvalidOperationException(
                        "Connection string 'MusicCatalogDb' not found.");

                options.UseNpgsql(connectionString);
                options.AddInterceptors(interceptor);
            });

            builder.Services.AddDbContext<PodcastCatalogDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<PostgresRoleInterceptor>();

                var connectionString =
                    builder.Configuration.GetConnectionString("PodcastCatalogDb")
                    ?? throw new InvalidOperationException(
                        "Connection string 'PodcastCatalogDb' not found.");

                options.UseNpgsql(connectionString);
                options.AddInterceptors(interceptor);
            });

            builder.Services.AddDbContext<UserManagementDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<PostgresRoleInterceptor>();

                var connectionString =
                    builder.Configuration.GetConnectionString("UserManagementDb")
                    ?? throw new InvalidOperationException(
                        "Connection string 'UserManagementDb' not found.");

                options.UseNpgsql(connectionString);
                options.AddInterceptors(interceptor);
            });

            builder.Services.AddSingleton(Log.Logger);
            builder.Services.AddDbContext<MusicQualityControlDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<PostgresRoleInterceptor>();

                var connectionString =
                    builder.Configuration.GetConnectionString("MusicQualityControlDb")
                    ?? throw new InvalidOperationException(
                        "Connection string 'MusicQualityControlDb' not found.");

                options.UseNpgsql(connectionString);
                options.AddInterceptors(interceptor);
            });

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
            builder.Services.AddScoped<ILanguageRepository, MusicCatalogContext.Infrastructure.Repositories.LanguageRepository>();
            builder.Services.AddScoped<ICountryRepository, CountryRepository>();
            builder.Services.AddScoped<IGenreRepository, GenreRepository>();
            builder.Services.AddScoped<ITrackRepository, TrackRepository>();
            builder.Services.AddScoped<IContributorRepository, MusicCatalogContext.Infrastructure.Repositories.ContributorRepository>();
            builder.Services.AddScoped<IContributorRoleRepository, ContributionRoleRepository>();
            builder.Services.AddScoped<IReleaseRepository, ReleaseRepository>();
            builder.Services.AddScoped<IOutBoxRepository, OutboxRepository>();
            builder.Services.AddScoped<IUnitOfWork, MusicCatalogUnitOfWork>();
            builder.Services.AddScoped<ILabelRepository, LabelRepository>();


            // MUSIC QC
            builder.Services.AddScoped<IReleaseQcQueueItemRepository, ReleaseQcQueueItemRepository>();


            // 5. Repositories (podcast)
            builder.Services.AddScoped<IPodcastRepository, PodcastRepository>();
            builder.Services.AddScoped<PodcastCatalog.Features.Contributors.IContributorRepository,PodcastCatalog.Infrastructure.Repositories.ContributorRepository>();
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
                // Music Catalog
                cfg.RegisterServicesFromAssembly(typeof(GetLanguagesQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetGenresQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(CreateTrackCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(CreateContributorCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(CreateReleaseCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetContributorRolesQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetAllCountriesQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(CreateLabelCommand).Assembly);

                // Podcast
                cfg.RegisterServicesFromAssembly(typeof(CreatePodcastHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(UpdatePodcastHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetPodcastHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetAllPodcastsHandler).Assembly);

                // UserManagement
                cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetUsersQuery).Assembly);

                // Music QC
                cfg.RegisterServicesFromAssembly(typeof(SubmitReleaseToQcCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetQcQueueQuery).Assembly);
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
            //app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<UserEnrichmentMiddleware>();

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

            if (musicQualityControlConnectionString is null)
            {
                Log.Warning("MusicCatalog connection string not loaded");
            }
            else
            {
                Log.Information(
                    "MusicQualityControll connection loaded: {ConnectionString}",
                    SanitizeConnectionString(musicQualityControlConnectionString!));
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

                await app.RunAsync();
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

            for (var i = 0; i < parts.Length; i++)
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