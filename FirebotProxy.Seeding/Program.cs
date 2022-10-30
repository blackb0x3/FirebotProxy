using AutoBogus;
using CommandLine;
using FirebotProxy.Data.Access;
using FirebotProxy.Data.Entities;
using MoreLinq;

namespace FirebotProxy.Seeding;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var ctxFactory = new FirebotProxyContextFactory();
        var ctx = ctxFactory.CreateDbContext(args);

        await Parser.Default
            .ParseArguments<Options>(args)
            .WithParsedAsync(async opts => await RunOptions(opts, ctx));
    }

    private static async Task RunOptions(Options options, FirebotProxyContext ctx)
    {
        var usernameFaker = new Bogus.DataSets.Internet();
        var usernamesToPickFrom = new List<string>();

        for (var i = 0; i < options.AudienceSize; i++)
        {
            usernamesToPickFrom.Add(usernameFaker.UserName());
        }

        var msgFaker = new AutoFaker<ChatMessage>();
        msgFaker.RuleFor(cm => cm.SenderUsername, faker => faker.PickRandom(usernamesToPickFrom));
        msgFaker.RuleFor(cm => cm.Timestamp, faker => faker.Date.Between(options.EarliestTimestamp ?? DateTime.Today.AddMonths(-1), DateTime.Today));
        msgFaker.RuleFor(cm => cm.Content, faker => string.Join(". ", faker.Lorem.Sentences(faker.Random.Int(min: 1, max: 3))));

        const int batchSize = 10000;
        var totalBatches = options.AmountToGenerate / batchSize;
        var batchCounter = 0;

        foreach (var batch in msgFaker.GenerateLazy(options.AmountToGenerate).Batch(batchSize))
        {
            Console.WriteLine($"Batch {++batchCounter} of {totalBatches}");
            await ctx.AddRangeAsync(batch);
            await ctx.SaveChangesAsync();
            Console.WriteLine("Batch saved");
            Console.WriteLine(new string('-', Console.BufferWidth / 4));
        }
    }
}