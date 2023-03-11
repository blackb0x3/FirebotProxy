using MediatR;

namespace FirebotProxy.SecondaryPorts.RemoveExpiredChatMessages;

public class RemoveExpiredChatMessagesCommand : IRequest
{
    public DateTime Cutoff { get; set; }
}