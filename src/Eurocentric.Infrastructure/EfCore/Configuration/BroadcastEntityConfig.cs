using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.EfCore.Configuration;

internal sealed class BroadcastEntityConfig : IEntityTypeConfiguration<Broadcast>
{
    public void Configure(EntityTypeBuilder<Broadcast> builder)
    {
        builder.ToTable(DbConstants.TableNames.Broadcast);

        builder.Property(broadcast => broadcast.Id)
            .HasConversion(src => src.Value, value => BroadcastId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(broadcast => broadcast.ContestId)
            .HasConversion(src => src.Value, value => ContestId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(broadcast => broadcast.ContestStage)
            .HasColumnName("contest_stage")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(broadcast => broadcast.Status)
            .HasColumnName("broadcast_status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.OwnsMany(broadcast => broadcast.Competitors, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Juries, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Televotes, ConfigureAsTable);

        builder.HasKey(broadcast => broadcast.Id).IsClustered();

        builder.HasAlternateKey(broadcast => new { broadcast.ContestId, broadcast.ContestStage });

        builder.ToTable(AddContestStageEnumCheckConstraint);

        builder.ToTable(AddBroadcastStatusEnumCheckConstraint);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Competitor> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.BroadcastCompetitor);

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);

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

        builder.ToTable(AddRunningOrderPositionCheckConstraint);

        builder.ToTable(AddFinishingPositionCheckConstraint);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, JuryAward> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.BroadcastCompetitorJuryAward);

        builder.Property<int>("Id")
            .IsRequired()
            .UseIdentityColumn(1)
            .ValueGeneratedOnAdd();

        builder.WithOwner().HasForeignKey("BroadcastId", "CompetingCountryId")
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

        builder.ToTable(AddCountryIdsCheckConstraint);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, TelevoteAward> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.BroadcastCompetitorTelevoteAward);

        builder.Property<int>("Id")
            .IsRequired()
            .UseIdentityColumn(1)
            .ValueGeneratedOnAdd();

        builder.WithOwner().HasForeignKey("BroadcastId", "CompetingCountryId")
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

        builder.ToTable(AddCountryIdsCheckConstraint);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Jury> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.BroadcastJury);

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);

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

        builder.ToTable(DbConstants.TableNames.BroadcastTelevote);

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(televote => televote.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(televote => televote.PointsAwarded)
            .IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }

    private static void AddRunningOrderPositionCheckConstraint(OwnedNavigationTableBuilder<Broadcast, Competitor> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_running_order_position", "[running_order_position] > 0");

    private static void AddFinishingPositionCheckConstraint(OwnedNavigationTableBuilder<Broadcast, Competitor> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_finishing_position", "[finishing_position] > 0");

    private static void AddContestStageEnumCheckConstraint(TableBuilder<Broadcast> builder) =>
        builder.HasCheckConstraint("ck_broadcast_contest_stage_enum",
            $"[contest_stage] IN {EnumHelpers.GetSqlNameListInParentheses<ContestStage>()}");

    private static void AddBroadcastStatusEnumCheckConstraint(TableBuilder<Broadcast> builder) =>
        builder.HasCheckConstraint("ck_broadcast_broadcast_status_enum",
            $"[broadcast_status] IN {EnumHelpers.GetSqlNameListInParentheses<BroadcastStatus>()}");

    private static void AddPointsValueEnumCheckConstraint(OwnedNavigationTableBuilder<Competitor, JuryAward> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_jury_award_points_value_enum",
            $"[points_value] IN {EnumHelpers.GetSqlIntegerListInParentheses<PointsValue>()}");

    private static void AddPointsValueEnumCheckConstraint(OwnedNavigationTableBuilder<Competitor, TelevoteAward> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_televote_award_points_value_enum",
            $"[points_value] IN {EnumHelpers.GetSqlIntegerListInParentheses<PointsValue>()}");

    private static void AddCountryIdsCheckConstraint(OwnedNavigationTableBuilder<Competitor, JuryAward> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_jury_award_country_ids",
            "[competing_country_id] <> [voting_country_id]");

    private static void AddCountryIdsCheckConstraint(OwnedNavigationTableBuilder<Competitor, TelevoteAward> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_televote_award_country_ids",
            "[competing_country_id] <> [voting_country_id]");
}
