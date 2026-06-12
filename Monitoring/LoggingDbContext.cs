using Microsoft.EntityFrameworkCore;
using Monitoring.Models;

namespace Monitoring
{
    public partial class LoggingDbContext : DbContext
    {
        public LoggingDbContext()
        {
        }

        public LoggingDbContext(DbContextOptions<LoggingDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Gatewaylog> Gatewaylogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gatewaylog>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("gatewaylogs");

                entity.Property(e => e.Exception).HasColumnName("exception");
                entity.Property(e => e.Level).HasColumnName("level");
                entity.Property(e => e.LogEvent)
                    .HasColumnType("jsonb")
                    .HasColumnName("log_event");
                entity.Property(e => e.Message).HasColumnName("message");
                entity.Property(e => e.MessageTemplate).HasColumnName("message_template");
                entity.Property(e => e.Properties)
                    .HasColumnType("jsonb")
                    .HasColumnName("properties");
                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("timestamp");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
