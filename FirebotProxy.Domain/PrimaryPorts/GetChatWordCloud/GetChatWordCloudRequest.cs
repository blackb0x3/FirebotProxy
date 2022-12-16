using FirebotProxy.Domain.Representations;
using MediatR;
using OneOf;

namespace FirebotProxy.Domain.PrimaryPorts.GetChatWordCloud;

public class GetChatWordCloudRequest : IRequest<OneOf<GetChatWordCloudResponse, ValidationRepresentation, ErrorRepresentation>>
{
    public string? ViewerUsername { get; set; }

    public string? StreamDate { get; set; }

    public WordCloudSettings WordCloudSettings { get; set; }
}

public class WordCloudSettings
{
    public int Width { get; set; }

    public int Height { get; set; }

    public string BackgroundHexColour { get; set; }

    public string FontFamily { get; set; }

    public string[] WordHexColours { get; set; }
}