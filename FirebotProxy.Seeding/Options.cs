using CommandLine;

namespace FirebotProxy.Seeding;

public class Options
{
    [Option("amount", Required = true, HelpText = "Number of messages to seed.")]
    public int AmountToGenerate { get; set; }

    [Option("start-from", Required = false, HelpText = "Earliest date for message timestamps.")]
    public DateTime? EarliestTimestamp { get; set; }

    [Option("audience-size", Required = false, Default = 10)]
    public int AudienceSize { get; set; }
}