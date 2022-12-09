using FirebotProxy.Extensions;
using FirebotProxy.Infrastructure.Services;
using FirebotProxy.SecondaryPorts.GenerateWordCloud;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace FirebotProxy.Infrastructure.Adapters;

internal class GenerateWordCloudCommandHandler : IRequestHandler<GenerateWordCloudCommand, OneOf<GenerateWordCloudSuccess, GenerateWordCloudFailure>>
{
    private readonly ILogger<GenerateWordCloudCommandHandler> _logger;
    private readonly IQuickChartApi _quickChartApi;

    public GenerateWordCloudCommandHandler(ILogger<GenerateWordCloudCommandHandler> logger, IQuickChartApi quickChartApi)
    {
        _logger = logger;
        _quickChartApi = quickChartApi;
    }

    public async Task<OneOf<GenerateWordCloudSuccess, GenerateWordCloudFailure>> Handle(GenerateWordCloudCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var wordCloudContent = await _quickChartApi.GenerateWordCloud(request.WordCloudOptions);

            return new GenerateWordCloudSuccess { WordCloudContent = wordCloudContent };
        }
        catch (Exception e)
        {
            const string msg = "QuickChart API call failed";

            _logger.LogError(new
            {
                msg,
                request,
                handler = nameof(GenerateWordCloudCommandHandler),
                exception = e.Message,
                e.StackTrace
            });

            return new GenerateWordCloudFailure();
        }
    }
}