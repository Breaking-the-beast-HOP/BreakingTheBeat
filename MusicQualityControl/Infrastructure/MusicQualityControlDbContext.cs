using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using MusicQualityControl.Domain.ReadModels;

namespace MusicQualityControl.Infrastructure;

public class MusicQualityControlDbContext : DbContext
{
    public MusicQualityControlDbContext(DbContextOptions<MusicQualityControlDbContext> options): base(options)
    {
    }

    public DbSet<ReleaseQcQueueItem> ReleaseQcQueueItems => Set<ReleaseQcQueueItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicQualityControlDbContext).Assembly);
    }
}