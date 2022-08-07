namespace FirebotProxy.Helpers;

public class DatabasePathHelper
{
    private const string CacheDbFileName = "firebotproxy.db";

    public static string CreateDbFilePath()
    {
        var userHomeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var dbFilePath = Path.Join(userHomeFolder, CacheDbFileName);

        return dbFilePath;
    }

    public static string GetSqliteConnectionString() => $"Data Source={CreateDbFilePath()}";
}