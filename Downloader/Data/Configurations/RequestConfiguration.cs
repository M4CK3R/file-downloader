using Downloader.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Downloader.Data.Configurations;

public class RequestConfiguration : EntityConfiguration<Request>
{
    public override void Configure(EntityTypeBuilder<Request> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.Url).IsRequired();
        builder.HasIndex(e => e.Name).IsUnique();
        builder.Property(e => e.MaxTries);
        builder.Property(e => e.Retry);
        builder.Property(e => e.FinishedAt);
    }
}