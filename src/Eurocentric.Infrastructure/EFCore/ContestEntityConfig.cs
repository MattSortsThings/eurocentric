using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.EFCore;

internal sealed class ContestEntityConfig : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.ToTable(DbConstants.TableNames.Contest);

        builder.Property(contest => contest.Id)
            .HasConversion(src => src.Value, value => ContestId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(contest => contest.ContestYear)
            .HasConversion(src => src.Value, value => ContestYear.FromValue(value).Value)
            .IsRequired();

        builder.Property(contest => contest.CityName)
            .HasConversion(src => src.Value, value => CityName.FromValue(value).Value)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(contest => contest.ContestFormat)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(contest => contest.ContestStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.OwnsMany(contest => contest.ChildBroadcasts, ConfigureAsTable);

        builder.OwnsMany(contest => contest.Participants, ConfigureAsTable);

        builder.HasKey(contest => contest.Id).IsClustered();

        builder.HasAlternateKey(contest => contest.ContestYear);

        builder.HasDiscriminator(contest => contest.ContestFormat)
            .HasValue<StockholmFormatContest>(ContestFormat.Stockholm)
            .HasValue<LiverpoolFormatContest>(ContestFormat.Liverpool)
            .IsComplete();

        builder.ToTable(AddContestYearCheckConstraint);

        builder.ToTable(AddContestFormatEnumCheckConstraint);

        builder.ToTable(AddContestStatusEnumCheckConstraint);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, BroadcastMemo> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.ContestChildBroadcast);

        builder.Property<int>("Id")
            .UseIdentityColumn(1)
            .ValueGeneratedOnAdd();

        builder.Property(memo => memo.BroadcastId)
            .HasConversion(src => src.Value, value => BroadcastId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(memo => memo.ContestStage)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(memo => memo.BroadcastStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.WithOwner()
            .HasForeignKey("ContestId")
            .HasPrincipalKey(contest => contest.Id);

        builder.HasKey("Id").IsClustered();

        builder.HasIndex("ContestId", "ContestStage").IsUnique();

        builder.ToTable(AddContestStageEnumCheckConstraint);

        builder.ToTable(AddBroadcastStatusEnumCheckConstraint);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, Participant> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.ContestParticipant);

        builder.Property(participant => participant.ParticipatingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(participant => participant.ParticipantGroup)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(participant => participant.ActName)
            .HasConversion(src => src!.Value, value => ActName.FromValue(value).Value)
            .HasMaxLength(200);

        builder.Property(participant => participant.SongTitle)
            .HasConversion(src => src!.Value, value => SongTitle.FromValue(value).Value)
            .HasMaxLength(200);

        builder.WithOwner()
            .HasForeignKey("ContestId")
            .HasPrincipalKey(contest => contest.Id);

        builder.HasKey("ContestId", "ParticipatingCountryId").IsClustered();

        builder.ToTable(AddParticipantGroupEnumCheckConstraint);

        builder.ToTable(AddValueNullabilityCheckConstraint);
    }

    private static void AddContestYearCheckConstraint(TableBuilder<Contest> builder) =>
        builder.HasCheckConstraint("ck_contest_contest_year", "[contest_year] BETWEEN 2016 AND 2050");

    private static void AddContestFormatEnumCheckConstraint(TableBuilder<Contest> builder) =>
        builder.HasCheckConstraint("ck_contest_contest_format_enum",
            $"[contest_format] IN {EnumHelpers.GetSqlIntegerListInParentheses<ContestFormat>()}");

    private static void AddContestStatusEnumCheckConstraint(TableBuilder<Contest> builder) =>
        builder.HasCheckConstraint("ck_contest_contest_status_enum",
            $"[contest_status] IN {EnumHelpers.GetSqlIntegerListInParentheses<ContestStatus>()}");

    private static void AddContestStageEnumCheckConstraint(OwnedNavigationTableBuilder<Contest, BroadcastMemo> builder) =>
        builder.HasCheckConstraint("ck_contest_broadcast_memo_contest_stage_enum",
            $"[contest_stage] IN {EnumHelpers.GetSqlIntegerListInParentheses<ContestStage>()}");

    private static void AddBroadcastStatusEnumCheckConstraint(OwnedNavigationTableBuilder<Contest, BroadcastMemo> builder) =>
        builder.HasCheckConstraint("ck_contest_broadcast_memo_broadcast_status_enum",
            $"[broadcast_status] IN {EnumHelpers.GetSqlIntegerListInParentheses<BroadcastStatus>()}");

    private static void AddParticipantGroupEnumCheckConstraint(OwnedNavigationTableBuilder<Contest, Participant> builder) =>
        builder.HasCheckConstraint("ck_contest_participant_participant_group_enum",
            $"[participant_group] IN {EnumHelpers.GetSqlIntegerListInParentheses<ParticipantGroup>()}");

    private static void AddValueNullabilityCheckConstraint(OwnedNavigationTableBuilder<Contest, Participant> builder) =>
        builder.HasCheckConstraint("ck_contest_participant_value_nullability",
            "([participant_group] = 0 AND [act_name] IS NULL AND [song_title] IS NULL) " +
            "OR ([participant_group] <> 0 AND [act_name] IS NOT NULL AND [song_title] IS NOT NULL)");
}
