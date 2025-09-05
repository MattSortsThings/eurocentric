using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Infrastructure.DataAccess.EfCore.Config.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config.Aggregates;

internal sealed class ContestConfig : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.ToTable("contest");

        builder.Property(contest => contest.Id)
            .HasConversion<ContestIdConverter>()
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(contest => contest.ContestYear)
            .HasConversion<ContestYearConverter>()
            .IsRequired();

        builder.Property(contest => contest.CityName)
            .HasConversion<CityNameConverter>()
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(contest => contest.ContestFormat)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(contest => contest.Completed)
            .IsRequired();

        builder.OwnsMany(contest => contest.ChildBroadcasts, ConfigureAsTable);

        builder.OwnsMany(contest => contest.Participants, ConfigureAsTable);

        builder.OwnsOne(contest => contest.GlobalTelevote, ConfigureAsColumn);

        builder.HasKey(contest => contest.Id).IsClustered();

        builder.HasAlternateKey(contest => contest.ContestYear);

        builder.HasDiscriminator(contest => contest.ContestFormat)
            .HasValue<LiverpoolFormatContest>(ContestFormat.Liverpool)
            .HasValue<StockholmFormatContest>(ContestFormat.Stockholm)
            .IsComplete();

        builder.ToTable(AddContestYearCheckConstraint);

        builder.ToTable(AddContestFormatEnumCheckConstraint);

        builder.ToTable(AddGlobalTelevoteParticipatingCountryIdNullabilityCheckConstraint);
    }

    private static void AddContestYearCheckConstraint(TableBuilder<Contest> builder) =>
        builder.HasCheckConstraint("ck_contest_contest_year", "[contest_year] BETWEEN 2016 AND 2050");

    private static void AddContestFormatEnumCheckConstraint(TableBuilder<Contest> builder) =>
        builder.HasCheckConstraint("ck_contest_contest_year", "[contest_year] BETWEEN 2016 AND 2050");

    private static void AddGlobalTelevoteParticipatingCountryIdNullabilityCheckConstraint(TableBuilder<Contest> builder) =>
        builder.HasCheckConstraint("ck_contest_global_televote_nullability",
            "([contest_format] = 0 AND [global_televote_participating_country_id] IS NOT NULL) " +
            "OR ([contest_format] = 1 AND [global_televote_participating_country_id] IS NULL)");

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, ChildBroadcast> builder)
    {
        builder.ToTable("contest_child_broadcast");

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.WithOwner()
            .HasForeignKey("ContestId")
            .HasPrincipalKey(contest => contest.Id);

        builder.Property(childBroadcast => childBroadcast.BroadcastId)
            .HasConversion<BroadcastIdConverter>()
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(childBroadcast => childBroadcast.ContestStage)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(childBroadcast => childBroadcast.Completed)
            .IsRequired();

        builder.HasKey("ContestId", "BroadcastId").IsClustered();

        builder.HasIndex("ContestId", "ContestStage").IsUnique();

        builder.ToTable(AddContestStageEnumCheckConstraint);
    }

    private static void AddContestStageEnumCheckConstraint(OwnedNavigationTableBuilder<Contest, ChildBroadcast> builder) =>
        builder.HasCheckConstraint("ck_contest_child_broadcast_contest_stage_enum",
            "[contest_stage] BETWEEN 0 AND 2");

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, Participant> builder)
    {
        builder.ToTable("contest_participant");

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.WithOwner()
            .HasForeignKey("ContestId")
            .HasPrincipalKey(contest => contest.Id);

        builder.Property(participant => participant.ParticipatingCountryId)
            .HasConversion<CountryIdConverter>()
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(participant => participant.SemiFinalDraw)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(participant => participant.ActName)
            .HasConversion<ActNameConverter>()
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(participant => participant.SongTitle)
            .HasConversion<SongTitleConverter>()
            .HasMaxLength(200)
            .IsRequired();

        builder.HasKey("ContestId", "ParticipatingCountryId").IsClustered();

        builder.ToTable(AddSemiFinalDrawEnumCheckConstraint);
    }

    private static void AddSemiFinalDrawEnumCheckConstraint(OwnedNavigationTableBuilder<Contest, Participant> builder) =>
        builder.HasCheckConstraint("ck_contest_participant_semi_final_draw_enum",
            "[semi_final_draw] BETWEEN 0 AND 1");

    private static void ConfigureAsColumn(OwnedNavigationBuilder<Contest, GlobalTelevote> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.Property(globalTelevote => globalTelevote.ParticipatingCountryId)
            .HasColumnName("global_televote_participating_country_id")
            .HasConversion<CountryIdConverter>()
            .IsRequired()
            .ValueGeneratedNever();
    }
}
