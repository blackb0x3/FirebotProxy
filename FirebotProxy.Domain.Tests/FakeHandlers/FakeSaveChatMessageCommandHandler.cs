using FirebotProxy.SecondaryPorts.SaveChatMessage;
using MediatR;

namespace FirebotProxy.Domain.Tests.FakeHandlers;

public class FakeSaveChatMessageCommandHandler : IRequestHandler<SaveChatMessageCommand>
{
    private readonly bool _shouldThrowException;

    public FakeSaveChatMessageCommandHandler(bool shouldThrowException)
    {
        _shouldThrowException = shouldThrowException;
    }

    public async Task<Unit> Handle(SaveChatMessageCommand request, CancellationToken cancellationToken)
    {
        if (_shouldThrowException)
        {
            throw new Exception($"test exception message from {nameof(FakeSaveChatMessageCommandHandler)}");
        }

        return await Task.FromResult(Unit.Value);
    }
}