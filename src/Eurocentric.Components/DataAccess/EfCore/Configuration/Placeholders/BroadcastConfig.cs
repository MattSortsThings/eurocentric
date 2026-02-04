using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Placeholders.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Configuration.Placeholders;

internal sealed class BroadcastConfig : IEntityTypeConfiguration<Broadcast>
{
    public void Configure(EntityTypeBuilder<Broadcast> builder)
    {
        builder.ToTable("broadcast", DbSchemas.Placeholder);

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
            .Property(broadcast => broadcast.VotingFormat)
            .HasColumnName("voting_format")
            .HasConversion<string>()
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(broadcast => broadcast.Completed).HasColumnName("completed").IsRequired();

        builder.OwnsMany(broadcast => broadcast.Competitors, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Televotes, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Juries, ConfigureAsTable);

        builder.HasKey(broadcast => broadcast.Id).IsClustered();

        builder.HasAlternateKey(broadcast => broadcast.BroadcastDate);

        builder.HasAlternateKey(broadcast => new { broadcast.ParentContestId, broadcast.ContestStage });

        builder.ToTable(table =>
            table.HasCheckConstraint(
                "CK_broadcast_broadcast_date",
                "[broadcast_date] BETWEEN '2016-01-01' AND '2050-12-31'"
            )
        );
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Competitor> builder)
    {
        builder.ToTable("broadcast_competitor", DbSchemas.Placeholder);

        builder.Property<Guid>("BroadcastId").HasColumnName("broadcast_id").IsRequired().ValueGeneratedNever();

        builder
            .Property(competitor => competitor.CompetingCountryId)
            .HasColumnName("competing_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(competitor => competitor.PerformingSpot).HasColumnName("performing_spot").IsRequired();

        builder
            .Property(competitor => competitor.BroadcastHalf)
            .HasColumnName("broadcast_half")
            .HasConversion<string>()
            .HasMaxLength(6)
            .IsRequired()
            .IsUnicode(false);

        builder.Property(competitor => competitor.FinishingSpot).HasColumnName("finishing_spot").IsRequired();

        builder.OwnsMany(competitor => competitor.PointsAwards, ConfigureAsTable);

        builder.HasKey("BroadcastId", "CompetingCountryId").IsClustered();

        builder.HasIndex("BroadcastId", "PerformingSpot").IsUnique();

        builder.ToTable(table =>
            table.HasCheckConstraint("CK_broadcast_competitor_performing_spot", "[performing_spot] >= 1")
        );

        builder.ToTable(table =>
            table.HasCheckConstraint("CK_broadcast_competitor_finishing_spot", "[finishing_spot] >= 1")
        );

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, PointsAward> builder)
    {
        builder.ToTable("broadcast_competitor_points_award", DbSchemas.Placeholder);

        builder.Property<int>("RowId").HasColumnName("row_id").IsRequired().UseIdentityColumn().ValueGeneratedOnAdd();

        builder.Property<Guid>("BroadcastId").HasColumnName("broadcast_id").IsRequired().ValueGeneratedNever();

        builder
            .Property<Guid>("CompetingCountryId")
            .HasColumnName("competing_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(pointsAward => pointsAward.VotingCountryId)
            .HasColumnName("voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(pointsAward => pointsAward.VotingMethod)
            .HasColumnName("voting_method")
            .HasConversion<string>()
            .HasMaxLength(8)
            .IsRequired()
            .IsUnicode(false);

        builder.Property(pointsAward => pointsAward.PointsValue).HasColumnName("points_value").IsRequired();

        builder.HasKey("RowId").IsClustered();

        builder.ToTable(table =>
            table.HasCheckConstraint(
                "CK_broadcast_competitor_points_award_points_value",
                "[points_value] BETWEEN 0 AND 12"
            )
        );

        builder.ToTable(table =>
            table.HasCheckConstraint(
                "CK_broadcast_competitor_points_award_country_ids",
                "[competing_country_id] <> [voting_country_id]"
            )
        );

        builder.HasIndex("BroadcastId", "CompetingCountryId", "VotingCountryId", "VotingMethod").IsUnique();

        builder.HasIndex("CompetingCountryId");

        builder.HasIndex("VotingCountryId");

        builder
            .WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Televote> builder)
    {
        builder.ToTable("broadcast_televote", DbSchemas.Placeholder);

        builder.Property<Guid>("BroadcastId").HasColumnName("broadcast_id").IsRequired().ValueGeneratedNever();

        builder
            .Property(televote => televote.VotingCountryId)
            .HasColumnName("voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(televote => televote.PointsAwarded).HasColumnName("points_awarded").IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Jury> builder)
    {
        builder.ToTable("broadcast_jury", DbSchemas.Placeholder);

        builder.Property<Guid>("BroadcastId").HasColumnName("broadcast_id").IsRequired().ValueGeneratedNever();

        builder
            .Property(jury => jury.VotingCountryId)
            .HasColumnName("voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(jury => jury.PointsAwarded).HasColumnName("points_awarded").IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();

        builder.WithOwner().HasForeignKey("BroadcastId").HasPrincipalKey(broadcast => broadcast.Id);
    }
}
