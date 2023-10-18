using Downloader.Data.Configurations;
using Downloader.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Downloader.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Request> Requests { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        AddTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x is { Entity: Entity, State: EntityState.Added or EntityState.Modified });
        var now = DateTime.UtcNow;
        foreach (var entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
                ((Entity)entity.Entity).CreatedAt = now;
            }

            ((Entity)entity.Entity).UpdatedAt = now;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RequestConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}