using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Configuration;

internal sealed class BroadcastConfiguration : IEntityTypeConfiguration<Broadcast>
{
    public void Configure(EntityTypeBuilder<Broadcast> builder)
    {
        builder.ToTable("broadcast");

        builder.Property(broadcast => broadcast.Id)
            .HasConversion(src => src.Value, value => BroadcastId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(broadcast => broadcast.BroadcastDate)
            .HasColumnType("date")
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

        builder.HasIndex(broadcast => new { broadcast.ParentContestId, broadcast.ContestStage }).IsUnique();

        builder.ToTable(AddContestStageEnumCheckConstraint);
    }

    private static void AddContestStageEnumCheckConstraint(TableBuilder<Broadcast> builder) =>
        builder.HasCheckConstraint("ck_broadcast_contest_stage_enum",
            SqlHelpers.CreateEnumIntValueCheckConstraint<ContestStage>("contest_stage"));

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

        builder.Property(competitor => competitor.FinishingPosition)
            .IsRequired();

        builder.Property(competitor => competitor.RunningOrderPosition)
            .IsRequired();

        builder.OwnsMany(competitor => competitor.JuryAwards, ConfigureAsTable);

        builder.OwnsMany(competitor => competitor.TelevoteAwards, ConfigureAsTable);

        builder.HasKey("BroadcastId", "CompetingCountryId").IsClustered();

        builder.HasIndex("BroadcastId", "RunningOrderPosition").IsUnique();

        builder.ToTable(AddFinishingPositionCheckConstraint);

        builder.ToTable(AddRunningOrderPositionCheckConstraint);
    }

    private static void AddRunningOrderPositionCheckConstraint(OwnedNavigationTableBuilder<Broadcast, Competitor> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_running_order_position", "[running_order_position] > 0");

    private static void AddFinishingPositionCheckConstraint(OwnedNavigationTableBuilder<Broadcast, Competitor> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_finishing_position", "[finishing_position] > 0");

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

        builder.HasKey("Id").IsClustered();

        builder.HasIndex("BroadcastId", "CompetingCountryId", "VotingCountryId").IsUnique();

        builder.ToTable(AddPointsValueEnumCheckConstraint);

        builder.ToTable(AddCountryIdsCheckConstraintConstraint);
    }

    private static void AddPointsValueEnumCheckConstraint(OwnedNavigationTableBuilder<Competitor, JuryAward> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_jury_award_points_value_enum",
            SqlHelpers.CreateEnumIntValueCheckConstraint<PointsValue>("points_value"));

    private static void AddCountryIdsCheckConstraintConstraint(OwnedNavigationTableBuilder<Competitor, JuryAward> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_jury_award_country_ids",
            "[competing_country_id] <> [voting_country_id]");

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

        builder.HasKey("Id").IsClustered();

        builder.HasIndex("BroadcastId", "CompetingCountryId", "VotingCountryId").IsUnique();

        builder.ToTable(AddPointsValueEnumCheckConstraint);

        builder.ToTable(AddCountryIdsCheckConstraintConstraint);
    }

    private static void AddPointsValueEnumCheckConstraint(OwnedNavigationTableBuilder<Competitor, TelevoteAward> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_televote_award_points_value_enum",
            SqlHelpers.CreateEnumIntValueCheckConstraint<PointsValue>("points_value"));

    private static void AddCountryIdsCheckConstraintConstraint(OwnedNavigationTableBuilder<Competitor, TelevoteAward> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_televote_award_country_ids",
            "[competing_country_id] <> [voting_country_id]");

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Jury> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable("broadcast_jury");

        builder.WithOwner()
            .HasForeignKey("BroadcastId")
            .HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(voter => voter.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(voter => voter.PointsAwarded)
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

        builder.Property(voter => voter.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(voter => voter.PointsAwarded)
            .IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }
}
