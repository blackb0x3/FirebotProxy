namespace FirebotProxy.Extensions;

public static class DateTimeExtensions
{
    private static readonly Random Rnd = new();

    public static DateTime Random()
    {
        return Random(DateTime.UtcNow.AddMonths(-1));
    }

    public static DateTime Random(DateTime start)
    {
        return Random(start, DateTime.Today);
    }

    public static DateTime Random(DateTime start, DateTime end)
    {
        if (start > end)
        {
            throw new Exception("start date cannot be later than the end date");
        }

        // early exit if the 2 date times are exact, since either/or is the only option
        if (start.Equals(end))
        {
            return start;
        }

        var range = (end - start).Days;
        var rndDaysToAdd = Rnd.Next(range);

        return start.AddDays(rndDaysToAdd);
    }
}