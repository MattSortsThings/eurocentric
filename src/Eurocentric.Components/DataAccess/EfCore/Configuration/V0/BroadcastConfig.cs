using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Aggregates.V0.Broadcasts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Configuration.V0;

internal sealed class BroadcastConfig : IEntityTypeConfiguration<Broadcast>
{
    public void Configure(EntityTypeBuilder<Broadcast> builder)
    {
        builder.ToTable("broadcast", V0SchemaConstants.SchemaName);

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
            .IsRequired()
            .IsUnicode(false);

        builder
            .Property(broadcast => broadcast.BroadcastFormat)
            .HasColumnName("broadcast_format")
            .HasConversion<string>()
            .HasMaxLength(15)
            .IsRequired()
            .IsUnicode(false);

        builder.Property(broadcast => broadcast.Completed).HasColumnName("completed").IsRequired();

        builder
            .OwnsMany(broadcast => broadcast.Competitors, ConfigureAsTable)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder
            .OwnsMany(broadcast => broadcast.Juries, ConfigureAsTable)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder
            .OwnsMany(broadcast => broadcast.Televotes, ConfigureAsTable)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.HasKey(broadcast => broadcast.Id).IsClustered();

        builder.HasAlternateKey(broadcast => broadcast.BroadcastDate);

        builder.HasAlternateKey(broadcast => new { broadcast.ParentContestId, broadcast.ContestStage });

        builder.ToTable(AddBroadcastDateCheckConstraint);
    }

    private static void AddBroadcastDateCheckConstraint(TableBuilder<Broadcast> builder)
    {
        builder.HasCheckConstraint(
            "CK_broadcast_broadcast_date",
            "broadcast_date BETWEEN '2016-01-01' AND '2030-12-31'"
        );
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Competitor> builder)
    {
        builder.ToTable("broadcast_competitor", V0SchemaConstants.SchemaName);

        builder
            .Property<Guid>(ShadowPropertyNames.BroadcastId)
            .HasColumnName("broadcast_id")
            .IsRequired()
            .ValueGeneratedNever();

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

        builder
            .OwnsMany(competitor => competitor.PointsAwards, ConfigureAsTable)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.HasKey(ShadowPropertyNames.BroadcastId, nameof(Competitor.CompetingCountryId)).IsClustered();

        builder.WithOwner().HasForeignKey(ShadowPropertyNames.BroadcastId).HasPrincipalKey(broadcast => broadcast.Id);

        builder.ToTable(AddPerformingSpotCheckConstraint);

        builder.ToTable(AddFinishingSpotCheckConstraint);
    }

    private static void AddPerformingSpotCheckConstraint(OwnedNavigationTableBuilder<Broadcast, Competitor> builder) =>
        builder.HasCheckConstraint("CK_broadcast_competitor_performing_spot", "performing_spot >= 1");

    private static void AddFinishingSpotCheckConstraint(OwnedNavigationTableBuilder<Broadcast, Competitor> builder) =>
        builder.HasCheckConstraint("CK_broadcast_competitor_finishing_spot", "finishing_spot >= 1");

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, PointsAward> builder)
    {
        builder.ToTable("broadcast_competitor_points_award", V0SchemaConstants.SchemaName);

        builder
            .Property<int>(ShadowPropertyNames.RowId)
            .HasColumnName("row_id")
            .IsRequired()
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder
            .Property<Guid>(ShadowPropertyNames.BroadcastId)
            .HasColumnName("broadcast_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property<Guid>(ShadowPropertyNames.CompetingCountryId)
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

        builder.HasKey(ShadowPropertyNames.RowId).IsClustered();

        builder
            .HasIndex(
                ShadowPropertyNames.BroadcastId,
                ShadowPropertyNames.CompetingCountryId,
                nameof(PointsAward.VotingCountryId),
                nameof(PointsAward.VotingMethod)
            )
            .IsUnique();

        builder.HasIndex(ShadowPropertyNames.CompetingCountryId);

        builder.HasIndex(nameof(PointsAward.VotingCountryId));

        builder
            .WithOwner()
            .HasForeignKey(ShadowPropertyNames.BroadcastId, ShadowPropertyNames.CompetingCountryId)
            .HasPrincipalKey(ShadowPropertyNames.BroadcastId, ShadowPropertyNames.CompetingCountryId);

        builder.ToTable(AddPointsValueCheckConstraint);

        builder.ToTable(AddCountryIdsConstraint);
    }

    private static void AddPointsValueCheckConstraint(OwnedNavigationTableBuilder<Competitor, PointsAward> builder) =>
        builder.HasCheckConstraint(
            "CK_broadcast_competitor_points_award_points_value",
            "points_value BETWEEN 0 AND 12"
        );

    private static void AddCountryIdsConstraint(OwnedNavigationTableBuilder<Competitor, PointsAward> builder) =>
        builder.HasCheckConstraint(
            "CK_broadcast_competitor_points_award_country_ids",
            "competing_country_id <> voting_country_id"
        );

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Jury> builder)
    {
        builder.ToTable("broadcast_jury", V0SchemaConstants.SchemaName);

        builder
            .Property<Guid>(ShadowPropertyNames.BroadcastId)
            .HasColumnName("broadcast_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(jury => jury.VotingCountryId)
            .HasColumnName("voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(jury => jury.PointsAwarded).HasColumnName("points_awarded").IsRequired();

        builder.HasKey(ShadowPropertyNames.BroadcastId, nameof(Jury.VotingCountryId)).IsClustered();

        builder.WithOwner().HasForeignKey(ShadowPropertyNames.BroadcastId).HasPrincipalKey(broadcast => broadcast.Id);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Televote> builder)
    {
        builder.ToTable("broadcast_televote", V0SchemaConstants.SchemaName);

        builder
            .Property<Guid>(ShadowPropertyNames.BroadcastId)
            .HasColumnName("broadcast_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(televote => televote.VotingCountryId)
            .HasColumnName("voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(televote => televote.PointsAwarded).HasColumnName("points_awarded").IsRequired();

        builder.HasKey(ShadowPropertyNames.BroadcastId, nameof(Televote.VotingCountryId)).IsClustered();

        builder.WithOwner().HasForeignKey(ShadowPropertyNames.BroadcastId).HasPrincipalKey(broadcast => broadcast.Id);
    }

    private static class ShadowPropertyNames
    {
        public const string BroadcastId = "BroadcastId";
        public const string CompetingCountryId = "CompetingCountryId";
        public const string RowId = "RowId";
    }
}
