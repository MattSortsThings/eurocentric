using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Config;

internal sealed class BroadcastConfig : IEntityTypeConfiguration<Broadcast>
{
    public void Configure(EntityTypeBuilder<Broadcast> builder)
    {
        builder.ToTable(Tables.Dbo.Broadcast, Schemas.Dbo);

        builder
            .Property(broadcast => broadcast.Id)
            .HasColumnName("broadcast_id")
            .HasConversion(src => src.Value, value => BroadcastId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(broadcast => broadcast.BroadcastDate)
            .HasConversion(src => src.Value, value => BroadcastDate.FromValue(value).Value)
            .IsRequired();

        builder
            .Property(broadcast => broadcast.ParentContestId)
            .HasConversion(src => src.Value, value => ContestId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(broadcast => broadcast.ContestStage).HasConversion<string>().HasMaxLength(10).IsRequired();

        builder.Property(broadcast => broadcast.Completed).IsRequired();

        builder.OwnsMany(broadcast => broadcast.Competitors, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Juries, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Televotes, ConfigureAsTable);

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
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(Tables.Dbo.BroadcastCompetitor, Schemas.Dbo);

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);

        builder
            .Property(competitor => competitor.CompetingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(competitor => competitor.RunningOrderSpot)
            .HasConversion(src => src.Value, value => RunningOrderSpot.FromValue(value).Value)
            .IsRequired();

        builder
            .Property(competitor => competitor.FinishingPosition)
            .HasConversion(src => src.Value, value => FinishingPosition.FromValue(value).Value)
            .IsRequired();

        builder.OwnsMany(competitor => competitor.JuryAwards, ConfigureAsTable);
        builder.OwnsMany(competitor => competitor.TelevoteAwards, ConfigureAsTable);

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

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, JuryAward> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(Tables.Dbo.BroadcastCompetitorJuryAward, Schemas.Dbo);

        builder
            .WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder.Property<int>("RowId").IsRequired().UseIdentityColumn().ValueGeneratedOnAdd();

        builder
            .Property(award => award.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

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

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, TelevoteAward> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(Tables.Dbo.BroadcastCompetitorTelevoteAward, Schemas.Dbo);

        builder
            .WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder.Property<int>("RowId").IsRequired().UseIdentityColumn().ValueGeneratedOnAdd();

        builder
            .Property(award => award.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

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

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Jury> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(Tables.Dbo.BroadcastJury, Schemas.Dbo);

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);

        builder
            .Property(jury => jury.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(jury => jury.PointsAwarded).IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Televote> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(Tables.Dbo.BroadcastTelevote, Schemas.Dbo);

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);

        builder
            .Property(televote => televote.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(televote => televote.PointsAwarded).IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }
}
