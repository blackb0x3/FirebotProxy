using FirebotProxy.TestBase;
using FluentAssertions;
using NUnit.Framework;

namespace FirebotProxy.Helpers.Tests;

[TestFixture]
public class GetRankPositionFromInteger_Throws_Exception : TestFixtureBase
{
    [Test]
    public void When_Numeric_Position_Is_Zero()
    {
        Action act = () => { RankPositionHelper.GetRankPositionFromInteger(0); };

        act.Should().Throw<Exception>().WithMessage("Not a valid rank position integer. Should be greater than 0. (Parameter 'numericPosition')");
    }

    [Test]
    public void When_Numeric_Position_Is_Negative()
    {
        var negativeNumericPosition = Rnd.Next(int.MinValue, 0);

        Action act = () => { RankPositionHelper.GetRankPositionFromInteger(negativeNumericPosition); };

        act.Should().Throw<Exception>().WithMessage("Not a valid rank position integer. Should be greater than 0. (Parameter 'numericPosition')");
    }
}

[TestFixture]
public class GetRankPositionFromInteger_Returns_Rank_Position : TestFixtureBase
{
    [Test]
    [TestCase(1, "1st")]
    [TestCase(2, "2nd")]
    [TestCase(3, "3rd")]
    [TestCase(4, "4th")]
    [TestCase(5, "5th")]
    [TestCase(6, "6th")]
    [TestCase(7, "7th")]
    [TestCase(8, "8th")]
    [TestCase(9, "9th")]
    public void For_A_Single_Digit_Number(int singleDigitNumber, string expectedRankPosition)
    {
        var rankPosition = RankPositionHelper.GetRankPositionFromInteger(singleDigitNumber);

        rankPosition.Should().Be(expectedRankPosition);
    }

    [Test]
    [TestCase(11, "11th")]
    [TestCase(12, "12th")]
    [TestCase(13, "13th")]
    [TestCase(21, "21st")]
    [TestCase(22, "22nd")]
    [TestCase(23, "23rd")]
    [TestCase(42, "42nd")]
    [TestCase(69, "69th")]
    [TestCase(100, "100th")]
    [TestCase(101, "101st")]
    [TestCase(102, "102nd")]
    [TestCase(103, "103rd")]
    [TestCase(111, "111th")]
    [TestCase(112, "112th")]
    [TestCase(113, "113th")]
    [TestCase(121, "121st")]
    [TestCase(122, "122nd")]
    [TestCase(123, "123rd")]
    [TestCase(321, "321st")]
    [TestCase(248, "248th")]
    [TestCase(420, "420th")]
    [TestCase(666, "666th")]
    [TestCase(1000, "1000th")]
    [TestCase(1000000, "1000000th")]
    public void For_A_Multi_Digit_Number(int singleDigitNumber, string expectedRankPosition)
    {
        var rankPosition = RankPositionHelper.GetRankPositionFromInteger(singleDigitNumber);

        rankPosition.Should().Be(expectedRankPosition);
    }
}