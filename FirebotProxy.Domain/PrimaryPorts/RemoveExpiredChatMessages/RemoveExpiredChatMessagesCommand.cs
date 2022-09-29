using FirebotProxy.Domain.Representations;
using MediatR;
using OneOf;

namespace FirebotProxy.Domain.PrimaryPorts.RemoveExpiredChatMessages;

public class RemoveExpiredChatMessagesRequest : IRequest<OneOf<RemoveExpiredChatMessagesSuccess, ErrorRepresentation>>
{
}