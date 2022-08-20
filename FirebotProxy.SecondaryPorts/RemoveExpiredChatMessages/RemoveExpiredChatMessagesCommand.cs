using MediatR;

namespace FirebotProxy.SecondaryPorts.RemoveExpiredChatMessages;

public class RemoveExpiredChatMessagesCommand : IRequest<Unit>
{
    public DateTime Cutoff { get; set; }
}