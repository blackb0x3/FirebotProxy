using Newtonsoft.Json;

namespace FirebotProxy.Domain.InternalModels.GetViewerChatPlot;

public class ChartPayload<T>
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("data")]
    public Data<T> Data { get; set; }

    [JsonProperty("options")]
    public Options Options { get; set; }
}

public class Data<T>
{
    [JsonProperty("labels")]
    public List<string> Labels { get; set; }

    [JsonProperty("datasets")]
    public List<Dataset<T>> Datasets { get; set; }
}

public class Dataset<T>
{
    [JsonProperty("label")]
    public string Label { get; set; }

    [JsonProperty("backgroundColor")]
    public string BackgroundColor { get; set; }

    [JsonProperty("borderColor")]
    public string BorderColor { get; set; }

    [JsonProperty("data")]
    public List<T> Data { get; set; }

    [JsonProperty("fill")]
    public bool Fill { get; set; }

    [JsonProperty("lineTension")]
    public double LineTension { get; set; }
}

public class Options
{
    [JsonProperty("title")]
    public Title Title { get; set; }
}

public class Title
{
    [JsonProperty("display")]
    public bool Display { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }
}