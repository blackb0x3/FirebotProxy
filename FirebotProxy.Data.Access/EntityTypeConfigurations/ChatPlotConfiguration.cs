using FirebotProxy.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirebotProxy.Data.Access.EntityTypeConfigurations;

public class ChatPlotConfiguration : IEntityTypeConfiguration<ChatPlot>
{
    public void Configure(EntityTypeBuilder<ChatPlot> builder)
    {
        builder.ToTable("ChatPlots").HasKey(cp => cp.ViewerUsername);
    }
}