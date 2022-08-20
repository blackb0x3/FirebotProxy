using Microsoft.Extensions.Logging;

namespace FirebotProxy.Extensions;

public static class LoggerExtensions
{
    public static void LogDebug(this ILogger log, object values) => log.LogDebug("{@values}", values);

    public static void LogInfo(this ILogger log, object values) => log.LogInformation("{@values}", values);

    public static void LogWarning(this ILogger log, object values) => log.LogWarning("{@values}", values);

    public static void LogError(this ILogger log, object values) => log.LogError("{@values}", values);
}