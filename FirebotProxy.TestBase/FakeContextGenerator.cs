using FirebotProxy.Data.Access;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FirebotProxy.TestBase;

public class FakeContextGenerator
{
    public static FirebotProxyContext GenerateFakeContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<FirebotProxyContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));

        return new FirebotProxyContext(optionsBuilder.Options);
    }
}