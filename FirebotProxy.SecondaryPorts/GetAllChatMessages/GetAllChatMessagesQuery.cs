using FirebotProxy.Data.Entities;
using MediatR;

namespace FirebotProxy.SecondaryPorts.GetAllChatMessages;

public class GetAllChatMessagesQuery : IRequest<IReadOnlyCollection<ChatMessage>>
{
}