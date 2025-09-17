using Eurocentric.Domain.V0Entities;
using Eurocentric.Infrastructure.DataAccess.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config;

internal sealed class V0BroadcastConfig : IEntityTypeConfiguration<Broadcast>
{
    public void Configure(EntityTypeBuilder<Broadcast> builder)
    {
        builder.ToTable(V0Schema.Tables.Broadcast, V0Schema.Name);

        builder.Property(broadcast => broadcast.Id)
            .HasColumnName("broadcast_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(broadcast => broadcast.BroadcastDate)
            .IsRequired();

        builder.Property(broadcast => broadcast.ParentContestId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(broadcast => broadcast.ContestStage)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(broadcast => broadcast.Completed)
            .IsRequired();

        builder.OwnsMany(broadcast => broadcast.Competitors, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Juries, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Televotes, ConfigureAsTable);

        builder.HasKey(broadcast => broadcast.Id).IsClustered();

        builder.HasAlternateKey(broadcast => broadcast.BroadcastDate);

        builder.HasAlternateKey(broadcast => new { broadcast.ParentContestId, broadcast.ContestStage });
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Competitor> builder)
    {
        builder.ToTable(V0Schema.Tables.Competitor, V0Schema.Name);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner()
            .HasForeignKey("BroadcastId")
            .HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(competitor => competitor.CompetingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(competitor => competitor.RunningOrderSpot)
            .IsRequired();

        builder.Property(competitor => competitor.FinishingPosition)
            .IsRequired();

        builder.OwnsMany(competitor => competitor.JuryAwards, ConfigureAsTable);

        builder.OwnsMany(competitor => competitor.TelevoteAwards, ConfigureAsTable);

        builder.HasKey("BroadcastId", "CompetingCountryId").IsClustered();

        builder.HasIndex("BroadcastId", "RunningOrderSpot").IsUnique();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, JuryAward> builder)
    {
        builder.ToTable(V0Schema.Tables.JuryAward, V0Schema.Name);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder.Property<int>("RowId")
            .IsRequired()
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.Property(award => award.VotingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(award => award.PointsValue)
            .HasConversion<int>()
            .IsRequired();

        builder.HasKey("RowId").IsClustered();

        builder.HasIndex("BroadcastId", "CompetingCountryId", "VotingCountryId").IsUnique();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, TelevoteAward> builder)
    {
        builder.ToTable(V0Schema.Tables.TelevoteAward, V0Schema.Name);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder.Property<int>("RowId")
            .IsRequired()
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.Property(award => award.VotingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(award => award.PointsValue)
            .HasConversion<int>()
            .IsRequired();

        builder.HasKey("RowId").IsClustered();

        builder.HasIndex("BroadcastId", "CompetingCountryId", "VotingCountryId").IsUnique();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Jury> builder)
    {
        builder.ToTable(V0Schema.Tables.Jury, V0Schema.Name);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner()
            .HasForeignKey("BroadcastId")
            .HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(jury => jury.VotingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(jury => jury.PointsAwarded)
            .IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Televote> builder)
    {
        builder.ToTable(V0Schema.Tables.Televote, V0Schema.Name);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner()
            .HasForeignKey("BroadcastId")
            .HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(televote => televote.VotingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(televote => televote.PointsAwarded)
            .IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }
}
