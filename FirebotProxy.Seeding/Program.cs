using AutoBogus;
using CommandLine;
using FirebotProxy.Data.Access;
using FirebotProxy.Data.Entities;
using FirebotProxy.Seeding;

Console.WriteLine("Hello, World!");

var ctxFactory = new FirebotProxyContextFactory();
var ctx = ctxFactory.CreateDbContext(args);

await Parser.Default
    .ParseArguments<Options>(args)
    .WithParsedAsync(RunOptions);

async Task RunOptions(Options options)
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
    msgFaker.RuleFor(cm => cm.Content, faker => faker.Lorem.Letter(faker.Random.Number(1, 500)));

    await ctx.AddRangeAsync(msgFaker.GenerateLazy(options.AmountToGenerate));
    await ctx.SaveChangesAsync();
}