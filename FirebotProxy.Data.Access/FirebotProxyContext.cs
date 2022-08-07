using FirebotProxy.Data.Access.EntityTypeConfigurations;
using FirebotProxy.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FirebotProxy.Data.Access;

public class FirebotProxyContext : DbContext
{
    private FirebotProxyContext()
    {
    }

    public FirebotProxyContext(DbContextOptions<FirebotProxyContext> options) : base(options)
    {
    }

    public DbSet<ChatMessage> ChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ChatMessageConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}