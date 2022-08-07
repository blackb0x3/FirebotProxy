using FirebotProxy.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirebotProxy.Data.Access.EntityTypeConfigurations;

internal class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("ChatMessages").HasNoKey();
        builder.HasIndex(msg => msg.SenderUsername);
        builder.HasIndex(msg => msg.Timestamp);
    }
}