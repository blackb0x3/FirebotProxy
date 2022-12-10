using FirebotProxy.Infrastructure.Services;
using FirebotProxy.SecondaryPorts.ShortenUrl;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FirebotProxy.Infrastructure.Adapters;

public class ShortenUrlCommandHandler : IRequestHandler<ShortenUrlCommand, string>
{
    private readonly ILogger<ShortenUrlCommandHandler> _logger;
    private readonly IHideUriApi _hideUrlApi;

    public ShortenUrlCommandHandler(ILogger<ShortenUrlCommandHandler> logger, IHideUriApi hideUrlApi)
    {
        _logger = logger;
        _hideUrlApi = hideUrlApi;
    }

    public async Task<string> Handle(ShortenUrlCommand request, CancellationToken cancellationToken)
    {
        var resp = await _hideUrlApi.ShortenUrl(new ShortenUrlRequestDto { UrlToShorten = request.UrlToShorten });

        return resp.ShortenedUrl;
    }
}