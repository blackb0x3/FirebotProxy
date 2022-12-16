using FirebotProxy.Extensions;
using FirebotProxy.Infrastructure.Services;
using FirebotProxy.SecondaryPorts.ShortenUrl;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace FirebotProxy.Infrastructure.Adapters;

public class ShortenUrlCommandHandler : IRequestHandler<ShortenUrlCommand, OneOf<ShortenUrlSuccess, ShortenUrlFailure>>
{
    private readonly ILogger<ShortenUrlCommandHandler> _logger;
    private readonly IHideUriApi _hideUrlApi;

    public ShortenUrlCommandHandler(ILogger<ShortenUrlCommandHandler> logger, IHideUriApi hideUrlApi)
    {
        _logger = logger;
        _hideUrlApi = hideUrlApi;
    }

    public async Task<OneOf<ShortenUrlSuccess, ShortenUrlFailure>> Handle(ShortenUrlCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var resp = await _hideUrlApi.ShortenUrl(new ShortenUrlRequestDto { UrlToShorten = request.UrlToShorten });

            return new ShortenUrlSuccess { ShortenedUrl = resp.ShortenedUrl };
        }
        catch (Exception e)
        {
            const string msg = "HideUri API call failed";

            _logger.LogError(new
            {
                msg,
                request,
                handler = nameof(ShortenUrlCommandHandler),
                exception = e.Message,
                e.StackTrace
            });

            return new ShortenUrlFailure();
        }
    }
}