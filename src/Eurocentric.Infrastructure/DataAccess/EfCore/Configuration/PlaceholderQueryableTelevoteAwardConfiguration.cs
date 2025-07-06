using Eurocentric.Domain.Enums;
using Eurocentric.Domain.PlaceholderEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Configuration;

internal sealed class PlaceholderQueryableTelevoteAwardConfiguration : IEntityTypeConfiguration<QueryableTelevoteAward>
{
    public void Configure(EntityTypeBuilder<QueryableTelevoteAward> builder)
    {
        builder.ToTable("placeholder_queryable_televote_award");

        builder.Property(award => award.CompetingCountryCode)
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired();

        builder.Property(award => award.VotingCountryCode)
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired();

        builder.Property(award => award.ContestYear)
            .IsRequired();

        builder.Property(award => award.ContestStage)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(award => award.BroadcastTag)
            .HasMaxLength(6)
            .IsRequired();

        builder.Property(award => award.RunningOrderPosition)
            .IsRequired();

        builder.Property(award => award.PointsValue)
            .IsRequired();

        builder.Property(award => award.MaxPointsValue)
            .IsRequired();

        builder.Property(award => award.RealPointsValue)
            .IsRequired();

        builder.Property(award => award.NormalizedPointsValue)
            .IsRequired();

        builder.HasKey(award =>
            new { award.CompetingCountryCode, award.VotingCountryCode, award.ContestYear, award.ContestStage });

        builder.HasIndex(award => award.CompetingCountryCode);

        builder.HasIndex(award => award.VotingCountryCode);

        builder.HasIndex(award => award.ContestYear);

        builder.HasIndex(award => award.ContestStage);

        builder.ToTable(AddContestStageEnumCheckConstraint);
    }

    private static void AddContestStageEnumCheckConstraint(TableBuilder<QueryableTelevoteAward> builder) =>
        builder.HasCheckConstraint("ck_placeholder_queryable_televote_award_contest_stage_enum",
            SqlHelpers.CreateEnumIntValueCheckConstraint<ContestStage>("contest_stage"));
}
