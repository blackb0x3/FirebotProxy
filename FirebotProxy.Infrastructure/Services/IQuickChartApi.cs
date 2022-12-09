using FirebotProxy.SecondaryPorts.GenerateWordCloud;
using Refit;

namespace FirebotProxy.Infrastructure.Services;

public interface IQuickChartApi
{
    [Get("/wordcloud")]
    Task<string> GenerateWordCloud(WordCloudOptions options);
}