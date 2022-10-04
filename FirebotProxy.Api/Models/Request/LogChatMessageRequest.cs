namespace FirebotProxy.Api.Models.Request;

public class LogChatMessageRequest
{
    public string Content { get; set; } = null!;

    public string Timestamp { get; set; } = null!;

    public string SenderUsername { get; set; } = null!;

    public DateTime TimestampToDateTime => DateTime.Parse(Timestamp);
}