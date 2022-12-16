using FirebotProxy.SecondaryPorts.ShortenUrl;
using MediatR;
using OneOf;

namespace FirebotProxy.Domain.Tests.FakeHandlers;

public class FakeShortenUrlCommandHandler : IRequestHandler<ShortenUrlCommand, OneOf<ShortenUrlSuccess, ShortenUrlFailure>>
{
    private readonly bool _shouldFail;

    public FakeShortenUrlCommandHandler(bool shouldFail)
    {
        _shouldFail = shouldFail;
    }

    public async Task<OneOf<ShortenUrlSuccess, ShortenUrlFailure>> Handle(ShortenUrlCommand request, CancellationToken cancellationToken)
    {
        if (_shouldFail)
        {
            return await Task.FromResult(new ShortenUrlFailure());
        }

        return await Task.FromResult(new ShortenUrlSuccess { ShortenedUrl = "https://shortened.url/abcdef" });
    }
}