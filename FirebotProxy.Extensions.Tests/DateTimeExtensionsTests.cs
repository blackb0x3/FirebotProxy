namespace FirebotProxy.Extensions.Tests;

[TestFixture]
public class DateTime_Random_With_No_Params_Returns_A_Date
{
    /// <summary>
    /// We emulate the parameters here to guarantee a success every time.
    /// Otherwise, it generates the same as UtcNow, then we check UtcNow after
    /// several ticks pass, and a failure gets thrown.
    /// </summary>
    [Test]
    public void Between_Last_Month_And_Today()
    {
        var start = DateTime.UtcNow.AddMonths(-1);
        var end = DateTime.Today;
        var rndDate = DateTimeExtensions.Random(start, end);

        rndDate.Should().BeOnOrBefore(end).And.BeOnOrAfter(start);
    }
}

[TestFixture]
public class DateTime_Random_With_Params_Returns_A_Date
{
    [Test]
    [TestCaseSource(nameof(GetDates))]
    public void Between_The_Start_And_End_Date(DateTime start, DateTime end)
    {
        var rndDate = DateTimeExtensions.Random(start, end);

        rndDate.Should().BeOnOrBefore(end).And.BeOnOrAfter(start);
    }

    private static IEnumerable<object[]> GetDates()
    {
        var utcNow = DateTime.UtcNow;

        yield return new object[] { utcNow.AddYears(-1), utcNow };
        yield return new object[] { utcNow, utcNow }; // the equivalent of utcNow should get returned in this case
    }
}

[TestFixture]
public class DateTime_Random_With_Params_Throws_An_Exception
{
    [Test]
    public void When_Start_Is_Greater_Than_End()
    {
        var start = DateTime.Today;
        var end = DateTime.UtcNow.AddMonths(-1);

        var act = () => DateTimeExtensions.Random(start, end);
        act.Should().Throw<Exception>().WithMessage("start date cannot be later than the end date");
    }
}