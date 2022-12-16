using Bogus;
using Bogus.DataSets;
using FirebotProxy.SecondaryPorts.GetChatMessageText;
using MediatR;

namespace FirebotProxy.Domain.Tests.FakeHandlers;

public class FakeGetChatTextQueryHandler : IRequestHandler<GetChatMessageTextQuery, string>
{
    private readonly bool _shouldThrowException;

    public FakeGetChatTextQueryHandler(bool shouldThrowException)
    {
        _shouldThrowException = shouldThrowException;
    }

    public async Task<string> Handle(GetChatMessageTextQuery request, CancellationToken cancellationToken)
    {
        if (_shouldThrowException)
        {
            throw new Exception($"test exception message from {nameof(FakeGetChatTextQueryHandler)}");
        }

        return await Task.FromResult(new Lorem().Sentences(new Faker().Random.Number(1, 20)));
    }
}