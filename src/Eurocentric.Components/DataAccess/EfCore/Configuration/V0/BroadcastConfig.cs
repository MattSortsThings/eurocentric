using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Aggregates.V0;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Configuration.V0;

internal sealed class BroadcastConfig : IEntityTypeConfiguration<Broadcast>
{
    public void Configure(EntityTypeBuilder<Broadcast> builder)
    {
        builder.ToTable("broadcast", DbSchemas.V0);

        builder.Property(broadcast => broadcast.Id).HasColumnName("broadcast_id").IsRequired().ValueGeneratedNever();

        builder.Property(broadcast => broadcast.BroadcastDate).HasColumnName("broadcast_date").IsRequired();

        builder
            .Property(broadcast => broadcast.ParentContestId)
            .HasColumnName("parent_contest_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(broadcast => broadcast.ContestStage)
            .HasColumnName("contest_stage")
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

        builder
            .Property(broadcast => broadcast.VotingRules)
            .HasColumnName("voting_rules")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(broadcast => broadcast.Completed).HasColumnName("completed").IsRequired();

        builder.OwnsMany(broadcast => broadcast.Competitors, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Juries, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Televotes, ConfigureAsTable);

        builder.HasKey(broadcast => broadcast.Id).IsClustered();

        builder.HasIndex(broadcast => broadcast.BroadcastDate).IsUnique();

        builder.HasIndex(broadcast => new { broadcast.ParentContestId, broadcast.ContestStage }).IsUnique();

        builder.ToTable(table =>
            table.HasCheckConstraint(
                "CK_broadcast_broadcast_date",
                "[broadcast_date] BETWEEN '2016-01-01' AND '2050-12-31'"
            )
        );
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Competitor> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("broadcast_competitor", DbSchemas.V0);

        builder.Property<Guid>("BroadcastId").HasColumnName("broadcast_id").IsRequired().ValueGeneratedNever();

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);

        builder
            .Property(competitor => competitor.CompetingCountryId)
            .HasColumnName("competing_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(competitor => competitor.RunningOrderSpot).HasColumnName("running_order_spot").IsRequired();

        builder.Property(competitor => competitor.FinishingPosition).HasColumnName("finishing_position").IsRequired();

        builder.OwnsMany(competitor => competitor.JuryAwards, ConfigureAsTable);

        builder.OwnsMany(competitor => competitor.TelevoteAwards, ConfigureAsTable);

        builder.HasKey("BroadcastId", "CompetingCountryId").IsClustered();

        builder.HasIndex("BroadcastId", "RunningOrderSpot").IsUnique();

        builder.ToTable(table =>
            table.HasCheckConstraint("CK_broadcast_competitor_running_order_spot", "[running_order_spot] >= 1")
        );

        builder.ToTable(table =>
            table.HasCheckConstraint("CK_broadcast_competitor_finishing_position", "[finishing_position] >= 1")
        );
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Jury> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("broadcast_jury", DbSchemas.V0);

        builder.Property<Guid>("BroadcastId").HasColumnName("broadcast_id").IsRequired().ValueGeneratedNever();

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);

        builder
            .Property(jury => jury.VotingCountryId)
            .HasColumnName("voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(jury => jury.PointsAwarded).HasColumnName("points_awarded").IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Televote> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("broadcast_televote", DbSchemas.V0);

        builder.Property<Guid>("BroadcastId").HasColumnName("broadcast_id").IsRequired().ValueGeneratedNever();

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);

        builder
            .Property(televote => televote.VotingCountryId)
            .HasColumnName("voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(televote => televote.PointsAwarded).HasColumnName("points_awarded").IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, JuryAward> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("broadcast_competitor_jury_award", DbSchemas.V0);

        builder.Property<int>("RowId").HasColumnName("row_id").IsRequired().UseIdentityColumn().ValueGeneratedOnAdd();

        builder.Property<Guid>("BroadcastId").HasColumnName("broadcast_id").IsRequired().ValueGeneratedNever();

        builder
            .Property<Guid>("CompetingCountryId")
            .HasColumnName("competing_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder
            .Property(award => award.VotingCountryId)
            .HasColumnName("voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(award => award.PointsValue).HasColumnName("points_value").IsRequired();

        builder.HasKey("RowId").IsClustered();

        builder.HasIndex("BroadcastId").IsUnique(false);

        builder.HasIndex("CompetingCountryId").IsUnique(false);

        builder.HasIndex("VotingCountryId").IsUnique(false);

        builder.HasIndex("BroadcastId", "CompetingCountryId", "VotingCountryId").IsUnique();

        builder.ToTable(table =>
            table.HasCheckConstraint("CK_broadcast_competitor_jury_award_points_value", "[points_value] >= 0")
        );

        builder.ToTable(table =>
            table.HasCheckConstraint(
                "CK_broadcast_competitor_jury_award_country_ids",
                "[competing_country_id] <> [voting_country_id]"
            )
        );
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, TelevoteAward> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("broadcast_competitor_televote_award", DbSchemas.V0);

        builder.Property<int>("RowId").HasColumnName("row_id").IsRequired().UseIdentityColumn().ValueGeneratedOnAdd();

        builder.Property<Guid>("BroadcastId").HasColumnName("broadcast_id").IsRequired().ValueGeneratedNever();

        builder
            .Property<Guid>("CompetingCountryId")
            .HasColumnName("competing_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder
            .Property(award => award.VotingCountryId)
            .HasColumnName("voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(award => award.PointsValue).HasColumnName("points_value").IsRequired();

        builder.HasKey("RowId").IsClustered();

        builder.HasIndex("BroadcastId").IsUnique(false);

        builder.HasIndex("CompetingCountryId").IsUnique(false);

        builder.HasIndex("VotingCountryId").IsUnique(false);

        builder.HasIndex("BroadcastId", "CompetingCountryId", "VotingCountryId").IsUnique();

        builder.ToTable(table =>
            table.HasCheckConstraint("CK_broadcast_competitor_televote_award_points_value", "[points_value] >= 0")
        );

        builder.ToTable(table =>
            table.HasCheckConstraint(
                "CK_broadcast_competitor_televote_award_country_ids",
                "[competing_country_id] <> [voting_country_id]"
            )
        );
    }
}
