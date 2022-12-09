using System.Runtime.Serialization;

namespace FirebotProxy.SecondaryPorts.GenerateWordCloud;

public enum WordCloudScale
{
    [EnumMember(Value = "linear")]
    Linear,
    [EnumMember(Value = "sqrt")]
    SquareRoot,
    [EnumMember(Value = "log")]
    Log
}