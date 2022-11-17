using FirebotProxy.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FirebotProxy.Data.Access;

public class FirebotProxyContext : DbContext
{
    public FirebotProxyContext(DbContextOptions<FirebotProxyContext> options) : base(options)
    {
    }

    public DbSet<ChatMessage> ChatMessages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FirebotProxyContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateEntityTimestamps();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateEntityTimestamps()
    {
        var timestamp = DateTime.UtcNow;
        var entitiesToUpdate = ChangeTracker.Entries().Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entitiesToUpdate)
        {
            if (entry.State == EntityState.Added)
            {
                ((EntityBase)entry.Entity).Created = timestamp;
            }

            ((EntityBase)entry.Entity).LastUpdated = timestamp;
        }
    }
}