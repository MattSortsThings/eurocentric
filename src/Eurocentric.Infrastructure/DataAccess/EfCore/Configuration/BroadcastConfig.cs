using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Configuration;

internal sealed class BroadcastConfig : IEntityTypeConfiguration<Broadcast>
{
    public void Configure(EntityTypeBuilder<Broadcast> builder)
    {
        builder.ToTable("broadcast");

        builder.Property(broadcast => broadcast.Id)
            .HasConversion(src => src.Value, value => BroadcastId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(broadcast => broadcast.BroadcastDate)
            .HasConversion(src => src.Value, value => BroadcastDate.FromValue(value).Value)
            .IsRequired();

        builder.Property(broadcast => broadcast.ParentContestId)
            .HasConversion(src => src.Value, value => ContestId.FromValue(value))
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

        builder.ToTable(AddContestStageEnumCheckConstraint);
    }

    private static void AddContestStageEnumCheckConstraint(TableBuilder<Broadcast> builder) =>
        builder.HasCheckConstraint("ck_broadcast_contest_stage_enum", "[contest_stage] BETWEEN 0 AND 2");

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Competitor> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable("broadcast_competitor");

        builder.WithOwner()
            .HasForeignKey("BroadcastId")
            .HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(competitor => competitor.CompetingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(competitor => competitor.RunningOrderPosition)
            .IsRequired();

        builder.Property(competitor => competitor.FinishingPosition)
            .IsRequired();

        builder.OwnsMany(competitor => competitor.JuryAwards, ConfigureAsTable);

        builder.OwnsMany(competitor => competitor.TelevoteAwards, ConfigureAsTable);

        builder.HasKey("BroadcastId", "CompetingCountryId").IsClustered();

        builder.HasIndex("BroadcastId", "RunningOrderPosition").IsUnique();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, JuryAward> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable("broadcast_competitor_jury_award");

        builder.Property<int>("Id")
            .IsRequired()
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder.Property(award => award.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(award => award.PointsValue)
            .HasConversion<int>()
            .IsRequired();

        builder.ToTable(AddCountryIdsCheckConstraint);

        builder.ToTable(AddPointsValueEnumCheckConstraint);
    }

    private static void AddCountryIdsCheckConstraint(OwnedNavigationTableBuilder<Competitor, JuryAward> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_jury_award_country_ids",
            "[competing_country_id] <> [voting_country_id]");

    private static void AddPointsValueEnumCheckConstraint(OwnedNavigationTableBuilder<Competitor, JuryAward> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_jury_award_points_value_enum",
            "[points_value] IN (0,1,2,3,4,5,6,7,8,10,12)");

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, TelevoteAward> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable("broadcast_competitor_televote_award");

        builder.Property<int>("Id")
            .IsRequired()
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder.Property(award => award.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(award => award.PointsValue)
            .HasConversion<int>()
            .IsRequired();

        builder.ToTable(AddCountryIdsCheckConstraint);

        builder.ToTable(AddPointsValueEnumCheckConstraint);
    }

    private static void AddCountryIdsCheckConstraint(OwnedNavigationTableBuilder<Competitor, TelevoteAward> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_televote_award_country_ids",
            "[competing_country_id] <> [voting_country_id]");

    private static void AddPointsValueEnumCheckConstraint(OwnedNavigationTableBuilder<Competitor, TelevoteAward> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_televote_award_points_value_enum",
            "[points_value] IN (0,1,2,3,4,5,6,7,8,10,12)");

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Jury> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable("broadcast_jury");

        builder.WithOwner()
            .HasForeignKey("BroadcastId")
            .HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(jury => jury.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(jury => jury.PointsAwarded)
            .IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Televote> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable("broadcast_televote");

        builder.WithOwner()
            .HasForeignKey("BroadcastId")
            .HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(televote => televote.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(televote => televote.PointsAwarded)
            .IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }
}
