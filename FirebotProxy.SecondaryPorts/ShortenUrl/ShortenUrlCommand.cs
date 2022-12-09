using MediatR;

namespace FirebotProxy.SecondaryPorts.ShortenUrl;

public class ShortenUrlCommand : IRequest<string>
{
    public string UrlToShorten { get; set; }
}