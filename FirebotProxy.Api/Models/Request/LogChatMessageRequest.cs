namespace FirebotProxy.Api.Models.Request;

public class LogChatMessageRequest
{
    public string Content { get; set; }

    public string Timestamp { get; set; }

    public string SenderUsername { get; set; }

    public DateTime TimestampToDateTime => DateTime.Parse(Timestamp);
}