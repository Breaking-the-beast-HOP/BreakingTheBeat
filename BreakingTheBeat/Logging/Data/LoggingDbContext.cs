using Gateway.Logging.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Logging.Data
{
    public class LoggingDbContext : DbContext
    {
        public LoggingDbContext(DbContextOptions<LoggingDbContext> options) : base(options) { }

        public DbSet<GatewayLog> GatewayLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GatewayLog>(entity =>
            {
                // Primærnøgle
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn();

                // Felter
                entity.Property(e => e.RequestId)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.RequestName)
                    .HasMaxLength(255);

                entity.Property(e => e.Path)
                    .HasMaxLength(500);

                entity.Property(e => e.Method)
                    .HasMaxLength(10);

                entity.Property(e => e.RequestData)
                    .HasMaxLength(2000);

                entity.Property(e => e.LogLevel)
                    .HasMaxLength(20);

                entity.Property(e => e.Exception)
                    .HasMaxLength(4000);
            });
        }
    }
}