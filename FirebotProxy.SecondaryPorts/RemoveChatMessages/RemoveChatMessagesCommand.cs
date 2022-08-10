using MediatR;

namespace FirebotProxy.SecondaryPorts.RemoveChatMessages;

public class RemoveChatMessagesCommand : IRequest<RemoveChatMessagesResult>
{
    public DateTime Cutoff { get; set; }
}