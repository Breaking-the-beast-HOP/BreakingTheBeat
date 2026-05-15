using System.Security.Claims;
using System.Text;
using FluentValidation;
using Gateway.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicCatalog.Features;
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
using MusicCatalog.Infrastructure.UnitOfWorks;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Features.Podcasts.Interfaces;
using PodcastCatalog.Features.Podcasts.PodcastCore.Create;
using PodcastCatalog.Features.Podcasts.PodcastCore.Get;
using PodcastCatalog.Features.Podcasts.PodcastCore.GetAll;
using PodcastCatalog.Features.Podcasts.PodcastCore.Update;
using PodcastCatalog.Infrastructure.Data;
using PodcastCatalog.Infrastructure.Repositories;
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
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Auth context + PostgreSQL role switching
            builder.Services.AddHttpContextAccessor();

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
            builder.Services.AddScoped<IOutBoxRepository, OutboxRepository>();
            builder.Services.AddScoped<IUnitOfWork, MusicCatalogUnitOfWork>();

            // 5. Repositories (podcast)
            builder.Services.AddScoped<IPodcastRepository, PodcastRepository>();
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

            // 8. Controllers
            builder.Services.AddControllers();

            // 9. Swagger/OpenAPI
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Seed / migrate podcast database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var podcastContext = services.GetRequiredService<PodcastCatalogDbContext>();

                    podcastContext.Database.Migrate();
                    //DbInitializer.Initialize(podcastContext);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            // Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}