using Eurocentric.Domain.V0.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config;

internal sealed class V0QueryableBroadcastConfig : IEntityTypeConfiguration<QueryableBroadcast>
{
    public void Configure(EntityTypeBuilder<QueryableBroadcast> builder)
    {
        builder.ToView("vw_queryable_broadcast", "v0");

        builder.Property(broadcast => broadcast.BroadcastDate).IsRequired();

        builder.Property(broadcast => broadcast.ContestYear).IsRequired();

        builder
            .Property(broadcast => broadcast.ContestStage)
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(broadcast => broadcast.Competitors).IsRequired();

        builder.Property(broadcast => broadcast.Juries).IsRequired();

        builder.Property(broadcast => broadcast.Televotes).IsRequired();

        builder.HasNoKey();
    }
}
