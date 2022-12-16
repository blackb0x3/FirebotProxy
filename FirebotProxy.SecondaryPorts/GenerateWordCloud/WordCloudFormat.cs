using System.Runtime.Serialization;

namespace FirebotProxy.SecondaryPorts.GenerateWordCloud;

public enum WordCloudFormat
{
    [EnumMember(Value = "svg")]
    Svg,
    [EnumMember(Value = "png")]
    Png
}