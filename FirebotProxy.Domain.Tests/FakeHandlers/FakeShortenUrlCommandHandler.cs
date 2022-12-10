using FirebotProxy.SecondaryPorts.ShortenUrl;
using MediatR;

namespace FirebotProxy.Domain.Tests.FakeHandlers;

public class FakeShortenUrlCommandHandler : IRequestHandler<ShortenUrlCommand, string>
{
    private readonly bool _shouldThrowException;

    public FakeShortenUrlCommandHandler(bool shouldThrowException)
    {
        _shouldThrowException = shouldThrowException;
    }

    public async Task<string> Handle(ShortenUrlCommand request, CancellationToken cancellationToken)
    {
        if (_shouldThrowException)
        {
            throw new Exception($"test exception message from {nameof(FakeShortenUrlCommandHandler)}");
        }

        return await Task.FromResult("https://shortened.url/abcdef");
    }
}