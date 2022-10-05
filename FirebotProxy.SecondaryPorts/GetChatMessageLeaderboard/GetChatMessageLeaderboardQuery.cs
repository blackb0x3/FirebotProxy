using MediatR;

namespace FirebotProxy.SecondaryPorts.GetChatMessageLeaderboard;

public class GetChatMessageLeaderboardQuery : IRequest<IQueryable<KeyValuePair<string, int>>>
{
}