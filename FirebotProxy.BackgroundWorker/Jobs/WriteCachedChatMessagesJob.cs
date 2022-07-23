using MediatR;
using Quartz;

namespace FirebotProxy.BackgroundWorker.Jobs;

public class WriteCachedChatMessagesJob : IJob
{
    private readonly ILogger<WriteCachedChatMessagesJob> _logger;
    private readonly IMediator _mediator;

    public WriteCachedChatMessagesJob(ILogger<WriteCachedChatMessagesJob> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Writing chat messages to file");
    }
}