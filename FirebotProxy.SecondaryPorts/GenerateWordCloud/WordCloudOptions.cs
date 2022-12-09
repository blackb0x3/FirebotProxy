using Newtonsoft.Json;
using Refit;

namespace FirebotProxy.SecondaryPorts.GenerateWordCloud;

public class WordCloudOptions
{
    public string Text { get; set; }

    public WordCloudFormat Format { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    [AliasAs("backgroundColor")]
    public string BackgroundHexColour { get; set; }

    public string FontFamily { get; set; }

    public bool LoadGoogleFonts { get; set; }

    public int FontScale { get; set; }

    public WordCloudScale Scale { get; set; }

    /// <remarks>
    /// In pixels!
    /// </remarks>
    [AliasAs("padding")]
    public int WordPadding { get; set; }

    /// <remarks>
    /// In degrees!
    /// </remarks>
    [AliasAs("rotation")]
    public int MaxWordRotation { get; set; }

    [AliasAs("maxNumWords")]
    public int MaxWordCount { get; set; }

    public int MinWordLength { get; set; }

    public WordCloudCase Case { get; set; }

    public string[] WordHexColours { get; set; }

    public bool RemoveStopwords { get; set; }

    public string Language { get; set; }

    [AliasAs("colors")]
    public string? ColoursQueryString => WordHexColours.Length == 0 ? null : JsonConvert.SerializeObject(WordHexColours);
}