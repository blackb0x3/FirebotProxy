using FirebotProxy.Domain.PrimaryPorts.RemoveExpiredChatMessages;
using FirebotProxy.Extensions;
using MediatR;
using Quartz;

namespace FirebotProxy.BackgroundWorker.Jobs;

public class RemoveExpiredChatMessagesJob : IJob
{
    private readonly ILogger<RemoveExpiredChatMessagesJob> _logger;
    private readonly IMediator _mediator;

    public RemoveExpiredChatMessagesJob(ILogger<RemoveExpiredChatMessagesJob> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var removeExpiredMessagesCommand = new RemoveExpiredChatMessagesCommand();

        var result = await _mediator.Send(removeExpiredMessagesCommand);

        result.Switch(
            success => _logger.LogInfo(new { msg = "Successfully removed expired messages" }),
            error => _logger.LogError(error)
        );
    }
}