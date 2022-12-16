using Refit;

namespace FirebotProxy.SecondaryPorts.GenerateWordCloud;

public class WordCloudOptions
{
    [AliasAs("text")]
    public string Text { get; set; }

    [AliasAs("format")]
    public WordCloudFormat? Format { get; set; }

    [AliasAs("width")]
    public int? Width { get; set; }

    [AliasAs("height")]
    public int? Height { get; set; }

    [AliasAs("backgroundColor")]
    public string? BackgroundHexColour { get; set; }

    [AliasAs("fontFamily")]
    public string? FontFamily { get; set; }

    [AliasAs("loadGoogleFonts")]
    public bool LoadGoogleFonts { get; set; } = true;

    [AliasAs("fontScale")]
    public int? FontScale { get; set; }

    [AliasAs("scale")]
    public WordCloudScale? Scale { get; set; }

    /// <remarks>
    /// In pixels!
    /// </remarks>
    [AliasAs("padding")]
    public int? WordPadding { get; set; }

    /// <remarks>
    /// In degrees!
    /// </remarks>
    [AliasAs("rotation")]
    public int? MaxWordRotation { get; set; }

    [AliasAs("maxNumWords")]
    public int? MaxWordCount { get; set; }

    [AliasAs("minWordLength")]
    public int? MinWordLength { get; set; }

    [AliasAs("case")]
    public WordCloudCase? Case { get; set; }

    [AliasAs("removeStopwords")]
    public bool? RemoveStopwords { get; set; } = true;

    [AliasAs("language")]
    public string? Language { get; set; }

    [AliasAs("colors")]
    public string? WordHexColours { get; set; }
}