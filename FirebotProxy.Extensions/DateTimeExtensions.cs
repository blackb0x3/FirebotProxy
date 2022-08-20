namespace FirebotProxy.Extensions;

public static class DateTimeExtensions
{
    private static readonly Random Rnd = new();

    public static DateTime Random()
    {
        var start = DateTime.UtcNow.AddMonths(-1);
        var range = (DateTime.Today - start).Days; 

        return start.AddDays(Rnd.Next(range));
    }
}