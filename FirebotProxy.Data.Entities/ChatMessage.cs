﻿namespace FirebotProxy.Data.Entities;

public class ChatMessage : EntityBase
{
    public Guid Id { get; set; }

    public string SenderUsername { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public string Content { get; set; } = null!;
}