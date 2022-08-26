using FirebotProxy.Data.Entities;
using MediatR;

namespace FirebotProxy.SecondaryPorts.GetChatMessageLeaderboard;

public class GetChatMessageLeaderboardQuery : IRequest<IOrderedQueryable<KeyValuePair<string, int>>>
{
}