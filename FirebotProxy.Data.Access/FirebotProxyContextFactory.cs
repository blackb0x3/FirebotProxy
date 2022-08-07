using FirebotProxy.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FirebotProxy.Data.Access;

public class FirebotProxyContextFactory : IDesignTimeDbContextFactory<FirebotProxyContext>
{
    public FirebotProxyContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FirebotProxyContext>()
            .UseSqlite(DatabasePathHelper.GetSqliteConnectionString());

        return new FirebotProxyContext(optionsBuilder.Options);
    }
}