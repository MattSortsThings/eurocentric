using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.EFCore;

internal sealed class BroadcastEntityConfig : IEntityTypeConfiguration<Broadcast>
{
    public void Configure(EntityTypeBuilder<Broadcast> builder)
    {
        builder.ToTable(DbConstants.TableNames.Broadcast);

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

        builder.Property(broadcast => broadcast.BroadcastStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.OwnsMany(broadcast => broadcast.Competitors, ConfigureAsTable);

        builder.OwnsMany(b => b.Juries, ConfigureAsTable);

        builder.OwnsMany(b => b.Televotes, ConfigureAsTable);

        builder.HasKey(broadcast => broadcast.Id).IsClustered();

        builder.HasAlternateKey(broadcast => new { broadcast.ParentContestId, broadcast.ContestStage });

        builder.ToTable(AddBroadcastDateCheckConstraint);

        builder.ToTable(AddContestStageEnumCheckConstraint);

        builder.ToTable(AddBroadcastStatusEnumCheckConstraint);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Competitor> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.BroadcastCompetitor);

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

        builder.WithOwner()
            .HasForeignKey("BroadcastId")
            .HasPrincipalKey(broadcast => broadcast.Id);

        builder.HasKey("BroadcastId", "CompetingCountryId").IsClustered();

        builder.HasIndex("BroadcastId", "RunningOrderPosition").IsUnique();

        builder.ToTable(AddFinishingPositionCheckConstraint);

        builder.ToTable(AddRunningOrderPositionCheckConstraint);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, JuryAward> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.BroadcastCompetitorJuryAward);

        builder.Property<int>("Id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(award => award.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(award => award.PointsValue)
            .HasConversion<int>()
            .IsRequired();

        builder.WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

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
            .ValueGeneratedOnAdd();

        builder.Property(award => award.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(award => award.PointsValue)
            .HasConversion<int>()
            .IsRequired();

        builder.WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder.HasKey("Id").IsClustered();

        builder.HasIndex("BroadcastId", "CompetingCountryId", "VotingCountryId").IsUnique();

        builder.ToTable(AddPointsValueEnumCheckConstraint);

        builder.ToTable(AddCountryIdsCheckConstraint);
    }

    private static void AddContestStageEnumCheckConstraint(TableBuilder<Broadcast> builder) =>
        builder.HasCheckConstraint("ck_broadcast_contest_stage_enum",
            $"[contest_stage] IN {EnumHelpers.GetSqlIntegerListInParentheses<ContestStage>()}");

    private static void AddBroadcastStatusEnumCheckConstraint(TableBuilder<Broadcast> builder) =>
        builder.HasCheckConstraint("ck_broadcast_broadcast_status_enum",
            $"[broadcast_status] IN {EnumHelpers.GetSqlIntegerListInParentheses<BroadcastStatus>()}");

    private static void AddBroadcastDateCheckConstraint(TableBuilder<Broadcast> builder) =>
        builder.HasCheckConstraint("ck_broadcast_broadcast_date",
            "[broadcast_date] BETWEEN '2016-01-01' AND '2050-12-31'");

    private static void AddFinishingPositionCheckConstraint(OwnedNavigationTableBuilder<Broadcast, Competitor> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_finishing_position",
            "[finishing_position] > 0");

    private static void AddRunningOrderPositionCheckConstraint(OwnedNavigationTableBuilder<Broadcast, Competitor> builder) =>
        builder.HasCheckConstraint("ck_broadcast_competitor_running_order_position",
            "[running_order_position] > 0");

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

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Jury> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.BroadcastJury);

        builder.Property(jury => jury.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(jury => jury.PointsAwarded)
            .IsRequired();

        builder.WithOwner()
            .HasForeignKey("BroadcastId")
            .HasPrincipalKey(broadcast => broadcast.Id);

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Televote> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.BroadcastTelevote);

        builder.Property(televote => televote.VotingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(televote => televote.PointsAwarded)
            .IsRequired();

        builder.WithOwner()
            .HasForeignKey("BroadcastId")
            .HasPrincipalKey(broadcast => broadcast.Id);

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }
}
