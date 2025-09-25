using Eurocentric.Domain.Enums;
using Eurocentric.Domain.V0.Aggregates.Contests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config;

internal sealed class V0ContestConfig : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.ToTable("contest", "v0");

        builder
            .HasDiscriminator(contest => contest.ContestRules)
            .HasValue<LiverpoolRulesContest>(ContestRules.Liverpool)
            .HasValue<StockholmRulesContest>(ContestRules.Stockholm);

        builder
            .Property(contest => contest.Id)
            .HasColumnName("contest_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(contest => contest.ContestYear).IsRequired();

        builder.Property(contest => contest.CityName).HasMaxLength(200).IsRequired();

        builder
            .Property(contest => contest.ContestRules)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(contest => contest.Queryable).IsRequired();

        builder.OwnsMany(contest => contest.ChildBroadcasts, ConfigureAsTable);

        builder.OwnsMany(contest => contest.Participants, ConfigureAsTable);

        builder.OwnsOne(contest => contest.GlobalTelevote, ConfigureAsColumn);

        builder.HasKey(contest => contest.Id).IsClustered();

        builder.HasAlternateKey(contest => contest.ContestYear).IsClustered(false);

        builder.ToTable(AddContestRulesEnumCheckConstraint);

        builder.ToTable(AddGlobalTelevoteNullabilityCheckConstraint);

        builder.ToTable(AddContestYearRangeCheckConstraint);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, ChildBroadcast> builder)
    {
        builder.ToTable("child_broadcast", "v0");

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner().HasForeignKey("ContestId").HasPrincipalKey(contest => contest.Id);

        builder
            .Property(broadcast => broadcast.ChildBroadcastId)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(broadcast => broadcast.ContestStage)
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(broadcast => broadcast.Completed).IsRequired();

        builder.HasKey("ContestId", "ChildBroadcastId").IsClustered();

        builder.HasIndex("ContestId", "ContestStage").IsUnique();

        builder.ToTable(AddContestStageEnumCheckConstraint);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, Participant> builder)
    {
        builder.ToTable("participant", "v0");

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner().HasForeignKey("ContestId").HasPrincipalKey(contest => contest.Id);

        builder
            .Property(participant => participant.ParticipatingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(participant => participant.SemiFinalDraw)
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(participant => participant.ActName).HasMaxLength(200).IsRequired();

        builder.Property(participant => participant.SongTitle).HasMaxLength(200).IsRequired();

        builder.HasKey("ContestId", "ParticipatingCountryId").IsClustered();

        builder.ToTable(AddSemiFinalDrawEnumCheckConstraint);
    }

    private static void ConfigureAsColumn(OwnedNavigationBuilder<Contest, GlobalTelevote> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder
            .Property(televote => televote.VotingCountryId)
            .IsRequired()
            .ValueGeneratedNever()
            .HasColumnName("global_televote_voting_country_id");
    }

    private static void AddContestRulesEnumCheckConstraint(TableBuilder<Contest> builder)
    {
        builder.HasCheckConstraint(
            "ck_contest_contest_rules_enum",
            "contest_rules IN (N'Liverpool', N'Stockholm', N'Stockholm')"
        );
    }

    private static void AddGlobalTelevoteNullabilityCheckConstraint(TableBuilder<Contest> builder)
    {
        builder.HasCheckConstraint(
            "ck_contest_global_televote_nullability",
            "(contest_rules = N'Liverpool' AND global_televote_voting_country_id IS NOT NULL) "
                + "OR (contest_rules = N'Stockholm' AND global_televote_voting_country_id IS NULL)"
        );
    }

    private static void AddContestYearRangeCheckConstraint(TableBuilder<Contest> builder)
    {
        builder.HasCheckConstraint(
            "ck_contest_contest_year_range",
            "contest_year BETWEEN 2016 AND 2050"
        );
    }

    private static void AddSemiFinalDrawEnumCheckConstraint(
        OwnedNavigationTableBuilder<Contest, Participant> builder
    )
    {
        builder.HasCheckConstraint(
            "ck_participant_semi_final_draw_enum",
            "semi_final_draw IN (N'SemiFinal1', N'SemiFinal2')"
        );
    }

    private static void AddContestStageEnumCheckConstraint(
        OwnedNavigationTableBuilder<Contest, ChildBroadcast> builder
    )
    {
        builder.HasCheckConstraint(
            "ck_child_broadcast_contest_stage_enum",
            "contest_stage IN (N'SemiFinal1', N'SemiFinal2', N'GrandFinal')"
        );
    }
}
