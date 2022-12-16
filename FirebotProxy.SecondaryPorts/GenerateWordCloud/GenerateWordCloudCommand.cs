using MediatR;
using OneOf;

namespace FirebotProxy.SecondaryPorts.GenerateWordCloud;

public class GenerateWordCloudCommand : IRequest<OneOf<GenerateWordCloudSuccess, GenerateWordCloudFailure>>
{
    public WordCloudOptions WordCloudOptions { get; set; }
}