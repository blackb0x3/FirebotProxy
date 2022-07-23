namespace FirebotProxy.SecondaryPorts.Models;

public class ChatMessage
{
    public string Content { get; set; }

    public DateTime Timestamp { get; set; }

    public string SenderUsername { get; set; }
}