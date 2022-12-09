using System.Runtime.Serialization;

namespace FirebotProxy.SecondaryPorts.GenerateWordCloud;

public enum WordCloudCase
{
    [EnumMember(Value = "none")]
    None,
    [EnumMember(Value = "upper")]
    Upper,
    [EnumMember(Value = "lower")]
    Lower
}