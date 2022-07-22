using FirebotProxy.Data.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirebotProxy.Data.Access;

public class FirebotProxyContext : DbContext
{
    public FirebotProxyContext(DbContextOptions<FirebotProxyContext> options) : base(options) { }

    public DbSet<CacheEntry> CacheEntries { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CacheEntriesConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        PreSaveChanges();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void PreSaveChanges()
    {
        var timestamp = DateTime.UtcNow;
        var entries = ChangeTracker.Entries().Where(x => x.State is EntityState.Added or EntityState.Modified);

        Parallel.ForEach(entries, e => UpdateTimestampProperties(e, timestamp));
    }

    private static void UpdateTimestampProperties(EntityEntry entry, DateTime timestamp)
    {
        if (entry.State == EntityState.Added)
        {
            ((EntityBase) entry.Entity).CreatedOn = timestamp;
        }

        ((EntityBase) entry.Entity).LastUpdatedOn = timestamp;
    }
}

internal class CacheEntriesConfiguration : IEntityTypeConfiguration<CacheEntry>
{
    public void Configure(EntityTypeBuilder<CacheEntry> builder)
    {
        builder.ToTable(nameof(FirebotProxyContext.CacheEntries)).HasKey(item => item.Key);
        builder.Property(item => item.Key).ValueGeneratedNever();

        builder.HasIndex(item => item.CreatedOn);
        builder.HasIndex(item => item.LastUpdatedOn);
    }
}