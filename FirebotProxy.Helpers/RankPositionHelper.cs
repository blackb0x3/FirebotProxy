namespace FirebotProxy.Helpers;

public class RankPositionHelper
{
    /// <summary>
    /// Gets string-ified rank position from an integer. E.g. 1 to 1st, 2 to 2nd, 3 to 3rd, 4 to 4th, 11 to 11th, 23 to 23rd etc.
    /// </summary>
    /// <param name="numericPosition"></param>
    /// <returns></returns>
    public static string GetRankPositionFromInteger(int numericPosition)
    {
        if (numericPosition <= 0)
        {
            throw new ArgumentException("Not a valid rank position integer. Should be greater than 0.", nameof(numericPosition));
        }

        switch (numericPosition % 100)
        {
            case 11:
            case 12:
            case 13:
                return $"{numericPosition}th";
        }

        switch (numericPosition % 10)
        {
            case 1:
                return $"{numericPosition}st";
            case 2:
                return $"{numericPosition}nd";
            case 3:
                return $"{numericPosition}rd";
            default:
                return $"{numericPosition}th";
        }
    }
}