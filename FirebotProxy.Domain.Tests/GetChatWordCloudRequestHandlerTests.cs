using FirebotProxy.Domain.PrimaryPorts.GetChatWordCloud;
using NUnit.Framework;

namespace FirebotProxy.Domain.Tests;

public class GetChatWordCloudRequestHandlerTestsBase
{
    protected GetChatWordCloudRequest BuildRequest()
    {
        return new GetChatWordCloudRequest
        {
            WordCloudSettings = new WordCloudSettings
            {
                Width = 100,
                Height = 100,
                BackgroundHexColour = "#ffffff",
                FontFamily = "Arial",
                WordHexColours = Array.Empty<string>()
            }
        };
    }
}

[TestFixture]
public class A_GetChatWordCloud_Request_Handler
{
    public class Generates_A_Word_Cloud : GetChatWordCloudRequestHandlerTestsBase
    {
        [Test]
        public async Task Normally()
        {
        }

        [Test]
        public async Task When_There_Is_No_Viewer_Username_In_The_Request()
        {
        }

        [Test]
        public async Task When_There_Is_No_Stream_Date_In_The_Request()
        {
        }

        [Test]
        public async Task When_There_Is_No_Viewer_Username_Or_Stream_Date_In_The_Request()
        {
        }

        [Test]
        public async Task When_The_Word_Cloud_Font_Family_Is_A_Standard_Web_Font()
        {
        }

        [Test]
        public async Task When_The_Word_Cloud_Font_Family_Is_A_Google_Font()
        {
        }
    }

    public class Does_Not_Generate_A_Word_Cloud : GetChatWordCloudRequestHandlerTestsBase
    {
        [Test]
        public async Task When_The_Request_Is_Invalid()
        {
        }

        [Test]
        public async Task When_The_Chat_Text_Cannot_Be_Retrieved()
        {
        }

        [Test]
        public async Task When_The_Word_Cloud_Cannot_Be_Generated()
        {
        }

        [Test]
        public async Task When_The_Link_To_The_Word_Cloud_Cannot_Be_Shortened()
        {
        }
    }
}