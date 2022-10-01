namespace FirebotProxy.Extensions.Tests;

public enum TestEnum
{
    TestEnumNoDescription,
    [System.ComponentModel.Description("Test Enum With Description")]
    TestEnumWithDescription,
}

[TestFixture]
public class EnumExtensions_GetDescription_Describes_An_Enum
{
    [Test]
    public void When_A_Description_Is_Present()
    {
        TestEnum.TestEnumWithDescription.GetDescription().Should().Be("Test Enum With Description");
    }

    [Test]
    public void When_A_Description_Is_Not_Present()
    {
        TestEnum.TestEnumNoDescription.GetDescription().Should().Be("TestEnumNoDescription");
    }
}