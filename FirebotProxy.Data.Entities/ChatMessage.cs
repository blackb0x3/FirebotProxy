namespace FirebotProxy.Data.Entities;

public class ChatMessage : EntityBase
{
    public string SenderUsername { get; set; }

    public DateTime Timestamp { get; set; }

    public string Content { get; set; }
}