using Microsoft.AspNetCore.Mvc;

namespace FirebotProxy.Api.Models.Request;

public class GetChatWordCloudRequestModel
{
    public string? ViewerUsername { get; set; }

    public string? StreamDate { get; set; }

    public WordCloudSettingsModel WordCloudSettings { get; set; }
}

public class WordCloudSettingsModel
{
    public int Width { get; set; }

    public int Height { get; set; }

    public string BackgroundHexColour { get; set; }

    public string FontFamily { get; set; }

    public string[] WordHexColours { get; set; }
}