using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FirebotProxy.Data.Access;

public class FirebotProxyContextFactory : IDesignTimeDbContextFactory<FirebotProxyContext>
{
    public FirebotProxyContext CreateDbContext(string[] args)
    {
        var userHomeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var dbFilePath = Path.Join(userHomeFolder, "firebotproxy.db");

        if (!File.Exists(dbFilePath))
        {
            File.Create(dbFilePath).Dispose();
        }

        var optionsBuilder = new DbContextOptionsBuilder<FirebotProxyContext>()
            .UseSqlite($@"Data Source={dbFilePath};");

        return new FirebotProxyContext(optionsBuilder.Options);
    }
}