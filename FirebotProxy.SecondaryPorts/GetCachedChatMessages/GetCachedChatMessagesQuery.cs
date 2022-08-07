using FirebotProxy.Data.Entities;
using MediatR;

namespace FirebotProxy.SecondaryPorts.GetCachedChatMessages;

public class GetCachedChatMessagesQuery : IRequest<List<ChatMessage>>
{
}