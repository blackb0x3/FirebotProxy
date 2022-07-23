namespace FirebotProxy.Helpers;

public static class CacheHelper
{
    private const string CacheDbFileName = "firebotproxy.db";

    public static string CreateDistributedCachePath()
    {
        var userHomeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var dbFilePath = Path.Join(userHomeFolder, CacheDbFileName);

        return dbFilePath;
    }
}