namespace FirebotProxy.Extensions.Tests;

[TestFixture]
public class StringExtensions_ToUrlSafeBase64String
{
    [Test]
    public void Creates_A_Url_Safe_Base64_String()
    {
        var stringToEncode = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        stringToEncode.ToUrlSafeBase64String()
            .Should().NotContain("+")
            .And.NotContain("/")
            .And.NotContain("=");
    }
}