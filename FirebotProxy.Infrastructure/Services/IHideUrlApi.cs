using Newtonsoft.Json;
using Refit;

namespace FirebotProxy.Infrastructure.Services;

public interface IHideUriApi
{
    [Post("/api/v1/shorten")]
    Task<ShortenUrlResponseDto> ShortenUrl([Body(BodySerializationMethod.UrlEncoded)] ShortenUrlRequestDto dto);
}

public class ShortenUrlRequestDto
{
    [AliasAs("url")]
    public string UrlToShorten { get; set; }
}

public class ShortenUrlResponseDto
{
    [JsonProperty("result_url")]
    public string ShortenedUrl { get; set; }
}