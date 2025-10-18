using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.V0.Aggregates.Contests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Config.V0;

internal sealed class ContestConfig : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.ToTable(Tables.V0.Contest, Schemas.V0);

        builder.Property(contest => contest.Id).HasColumnName("contest_id").IsRequired().ValueGeneratedNever();

        builder.Property(contest => contest.ContestYear).IsRequired();

        builder.Property(contest => contest.CityName).HasMaxLength(200).IsRequired();

        builder.Property(contest => contest.ContestRules).HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.Property(contest => contest.Queryable).IsRequired();

        builder.OwnsMany(contest => contest.ChildBroadcasts, ConfigureAsTable);

        builder.OwnsMany(contest => contest.Participants, ConfigureAsTable);

        builder.OwnsOne(contest => contest.GlobalTelevote, ConfigureAsColumn);

        builder
            .HasDiscriminator(contest => contest.ContestRules)
            .HasValue<LiverpoolRulesContest>(ContestRules.Liverpool)
            .HasValue<StockholmRulesContest>(ContestRules.Stockholm);

        builder.HasKey(contest => contest.Id).IsClustered();

        builder.HasAlternateKey(contest => contest.ContestYear);

        builder.ToTable(AddContestYearCheckConstraint);

        builder.ToTable(AddGlobalTelevoteNullabilityCheckConstraint);
    }

    private static void AddContestYearCheckConstraint(TableBuilder<Contest> builder) =>
        builder.HasCheckConstraint("ck_contest_contest_year", "contest_year BETWEEN 2016 AND 2050");

    private static void AddGlobalTelevoteNullabilityCheckConstraint(TableBuilder<Contest> builder)
    {
        builder.HasCheckConstraint(
            "ck_contest_global_televote_nullability",
            "(contest_rules = N'Liverpool' AND global_televote_voting_country_id IS NOT NULL) "
                + "OR (contest_rules = N'Stockholm' AND global_televote_voting_country_id IS NULL)"
        );
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, ChildBroadcast> builder)
    {
        builder.ToTable(Tables.V0.ContestChildBroadcast, Schemas.V0);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner().HasForeignKey("ContestId").HasPrincipalKey(contest => contest.Id);

        builder.Property(broadcast => broadcast.ChildBroadcastId).IsRequired().ValueGeneratedNever();

        builder.Property(broadcast => broadcast.ContestStage).HasConversion<string>().HasMaxLength(10).IsRequired();

        builder.Property(broadcast => broadcast.Completed).IsRequired();

        builder.HasKey("ContestId", "ChildBroadcastId").IsClustered();

        builder.HasIndex("ContestId", "ContestStage").IsUnique();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, Participant> builder)
    {
        builder.ToTable(Tables.V0.ContestParticipant, Schemas.V0);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner().HasForeignKey("ContestId").HasPrincipalKey(contest => contest.Id);

        builder.Property(participant => participant.ParticipatingCountryId).IsRequired().ValueGeneratedNever();

        builder
            .Property(participant => participant.SemiFinalDraw)
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(participant => participant.ActName).HasMaxLength(200).IsRequired();

        builder.Property(participant => participant.SongTitle).HasMaxLength(200).IsRequired();

        builder.HasKey("ContestId", "ParticipatingCountryId").IsClustered();
    }

    private static void ConfigureAsColumn(OwnedNavigationBuilder<Contest, GlobalTelevote> builder)
    {
        builder
            .Property(televote => televote.VotingCountryId)
            .HasColumnName("global_televote_voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();
    }
}
