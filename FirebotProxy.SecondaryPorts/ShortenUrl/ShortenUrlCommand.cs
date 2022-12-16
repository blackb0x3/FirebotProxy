using MediatR;
using OneOf;

namespace FirebotProxy.SecondaryPorts.ShortenUrl;

public class ShortenUrlCommand : IRequest<OneOf<ShortenUrlSuccess, ShortenUrlFailure>>
{
    public string UrlToShorten { get; set; }
}