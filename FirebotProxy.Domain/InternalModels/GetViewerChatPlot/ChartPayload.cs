using Newtonsoft.Json;

namespace FirebotProxy.Domain.InternalModels.GetViewerChatPlot;

public class ChartPayload<T>
{
    [JsonProperty("type")]
    public string Type { get; set; } = null!;

    [JsonProperty("data")]
    public Data<T> Data { get; set; } = null!;

    [JsonProperty("options")]
    public Options Options { get; set; } = null!;
}

public class Data<T>
{
    [JsonProperty("labels")]
    public List<string> Labels { get; set; } = null!;

    [JsonProperty("datasets")]
    public List<Dataset<T>> Datasets { get; set; } = null!;
}

public class Dataset<T>
{
    [JsonProperty("label")]
    public string Label { get; set; } = null!;

    [JsonProperty("backgroundColor")]
    public string BackgroundColor { get; set; } = null!;

    [JsonProperty("borderColor")]
    public string BorderColor { get; set; } = null!;

    [JsonProperty("data")]
    public List<T> Data { get; set; } = null!;

    [JsonProperty("fill")]
    public bool Fill { get; set; }

    [JsonProperty("lineTension")]
    public double LineTension { get; set; }
}

public class Options
{
    [JsonProperty("title")]
    public Title Title { get; set; } = null!;
}

public class Title
{
    [JsonProperty("display")]
    public bool Display { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; } = null!;
}