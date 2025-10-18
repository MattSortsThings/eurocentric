using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.V0.Aggregates.Broadcasts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Config.V0;

internal sealed class BroadcastConfig : IEntityTypeConfiguration<Broadcast>
{
    public void Configure(EntityTypeBuilder<Broadcast> builder)
    {
        builder.ToTable(Tables.V0.Broadcast, Schemas.V0);

        builder.Property(broadcast => broadcast.Id).HasColumnName("broadcast_id").IsRequired().ValueGeneratedNever();

        builder.Property(broadcast => broadcast.BroadcastDate).IsRequired();

        builder.Property(broadcast => broadcast.ParentContestId).IsRequired().ValueGeneratedNever();

        builder.Property(broadcast => broadcast.ContestStage).HasConversion<string>().HasMaxLength(10).IsRequired();

        builder.Property(broadcast => broadcast.Completed).IsRequired();

        builder.OwnsMany(broadcast => broadcast.Competitors, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Televotes, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Juries, ConfigureAsTable);

        builder.HasKey(broadcast => broadcast.Id).IsClustered();

        builder.HasAlternateKey(broadcast => broadcast.BroadcastDate);

        builder.HasAlternateKey(broadcast => new { broadcast.ParentContestId, broadcast.ContestStage });

        builder.ToTable(AddBroadcastDateCheckConstraint);
    }

    private static void AddBroadcastDateCheckConstraint(TableBuilder<Broadcast> builder)
    {
        builder.HasCheckConstraint(
            "ck_broadcast_broadcast_date",
            "broadcast_date BETWEEN '2016-01-01' AND '2050-12-31'"
        );
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Competitor> builder)
    {
        builder.ToTable(Tables.V0.BroadcastCompetitor, Schemas.V0);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(competitor => competitor.CompetingCountryId).IsRequired().ValueGeneratedNever();

        builder.Property(competitor => competitor.RunningOrderSpot).IsRequired();

        builder.Property(competitor => competitor.FinishingPosition).IsRequired();

        builder.OwnsMany(competitor => competitor.TelevoteAwards, ConfigureAsTable);

        builder.OwnsMany(competitor => competitor.JuryAwards, ConfigureAsTable);

        builder.HasKey("BroadcastId", "CompetingCountryId").IsClustered();

        builder.HasIndex("BroadcastId", "RunningOrderSpot").IsUnique();

        builder.ToTable(AddRunningOrderSpotCheckConstraint);

        builder.ToTable(AddFinishingPositionCheckConstraint);
    }

    private static void AddRunningOrderSpotCheckConstraint(
        OwnedNavigationTableBuilder<Broadcast, Competitor> builder
    ) => builder.HasCheckConstraint("ck_broadcast_competitor_running_order_spot", "running_order_spot >= 1");

    private static void AddFinishingPositionCheckConstraint(
        OwnedNavigationTableBuilder<Broadcast, Competitor> builder
    ) => builder.HasCheckConstraint("ck_broadcast_competitor_finishing_position", "finishing_position >= 1");

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, TelevoteAward> builder)
    {
        builder.ToTable(Tables.V0.BroadcastCompetitorTelevoteAward, Schemas.V0);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder
            .WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder.Property<int>("RowId").IsRequired().UseIdentityColumn().ValueGeneratedOnAdd();

        builder.Property(award => award.VotingCountryId).IsRequired().ValueGeneratedNever();

        builder.Property(award => award.PointsValue).HasConversion<int>().IsRequired();

        builder.HasKey("RowId").IsClustered();

        builder.HasIndex("BroadcastId", "CompetingCountryId", "VotingCountryId").IsUnique();

        builder.ToTable(AddCountryIdsCheckConstraint);
    }

    private static void AddCountryIdsCheckConstraint(OwnedNavigationTableBuilder<Competitor, TelevoteAward> builder)
    {
        builder.HasCheckConstraint(
            "ck_broadcast_competitor_televote_award_country_ids",
            "competing_country_id <> voting_country_id"
        );
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, JuryAward> builder)
    {
        builder.ToTable(Tables.V0.BroadcastCompetitorJuryAward, Schemas.V0);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder
            .WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder.Property<int>("RowId").IsRequired().UseIdentityColumn().ValueGeneratedOnAdd();

        builder.Property(award => award.VotingCountryId).IsRequired().ValueGeneratedNever();

        builder.Property(award => award.PointsValue).HasConversion<int>().IsRequired();

        builder.HasKey("RowId").IsClustered();

        builder.HasIndex("BroadcastId", "CompetingCountryId", "VotingCountryId").IsUnique();

        builder.ToTable(AddCountryIdsCheckConstraint);
    }

    private static void AddCountryIdsCheckConstraint(OwnedNavigationTableBuilder<Competitor, JuryAward> builder)
    {
        builder.HasCheckConstraint(
            "ck_broadcast_competitor_jury_award_country_ids",
            "competing_country_id <> voting_country_id"
        );
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Televote> builder)
    {
        builder.ToTable(Tables.V0.BroadcastTelevote, Schemas.V0);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(televote => televote.VotingCountryId).IsRequired().ValueGeneratedNever();

        builder.Property(televote => televote.PointsAwarded).IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Jury> builder)
    {
        builder.ToTable(Tables.V0.BroadcastJury, Schemas.V0);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(jury => jury.VotingCountryId).IsRequired().ValueGeneratedNever();

        builder.Property(jury => jury.PointsAwarded).IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }
}
