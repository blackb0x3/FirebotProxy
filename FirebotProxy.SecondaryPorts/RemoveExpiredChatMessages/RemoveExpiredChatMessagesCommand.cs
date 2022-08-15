using MediatR;

namespace FirebotProxy.SecondaryPorts.RemoveExpiredChatMessages;

public class RemoveExpiredChatMessagesCommand : IRequest<RemoveExpiredChatMessagesResult>
{
    public DateTime Cutoff { get; set; }
}