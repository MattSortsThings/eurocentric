using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.EfCore.Configuration;

internal sealed class ContestEntityConfig : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.ToTable(DbConstants.TableNames.Contest);

        builder.Property(contest => contest.Id)
            .HasConversion(src => src.Value, value => ContestId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(contest => contest.Year)
            .HasColumnName("contest_year")
            .HasConversion(src => src.Value, value => ContestYear.FromValue(value).Value)
            .IsRequired();

        builder.Property(contest => contest.CityName)
            .HasConversion(src => src.Value, value => CityName.FromValue(value).Value)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(contest => contest.Format)
            .HasColumnName("contest_format")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(contest => contest.Status)
            .HasColumnName("contest_status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.OwnsMany(contest => contest.BroadcastMemos, ConfigureAsTable);
        builder.OwnsMany(contest => contest.Participants, ConfigureAsTable);

        builder.HasKey(contest => contest.Id).IsClustered();

        builder.HasAlternateKey(contest => contest.Year);

        builder.ToTable(AddContestYearCheckConstraint);

        builder.ToTable(AddContestFormatEnumCheckConstraint);

        builder.ToTable(AddContestStatusEnumCheckConstraint);

        builder.HasDiscriminator(contest => contest.Format)
            .HasValue<LiverpoolFormatContest>(ContestFormat.Liverpool)
            .HasValue<StockholmFormatContest>(ContestFormat.Stockholm);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, BroadcastMemo> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.ContestBroadcastMemo);

        builder.Property<int>("Id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.WithOwner()
            .HasForeignKey("ContestId")
            .HasPrincipalKey(contest => contest.Id);

        builder.Property(broadcastMemo => broadcastMemo.BroadcastId)
            .HasConversion(src => src.Value, value => BroadcastId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(broadcastMemo => broadcastMemo.ContestStage)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(broadcastMemo => broadcastMemo.Status)
            .HasColumnName("broadcast_status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasKey("Id").IsClustered();

        builder.HasIndex("ContestId", "BroadcastId").IsUnique();

        builder.HasIndex("ContestId", "ContestStage").IsUnique();

        builder.ToTable(AddContestStageEnumCheckConstraint);

        builder.ToTable(AddBroadcastStatusEnumCheckConstraint);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, Participant> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.ContestParticipant);

        builder.WithOwner()
            .HasForeignKey("ContestId")
            .HasPrincipalKey(contest => contest.Id);

        builder.Property(participant => participant.ParticipatingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(participant => participant.Group)
            .HasColumnName("participant_group")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(participant => participant.ActName)
            .HasConversion(src => src!.Value, value => ActName.FromValue(value).Value)
            .HasMaxLength(200);

        builder.Property(participant => participant.SongTitle)
            .HasConversion(src => src!.Value, value => SongTitle.FromValue(value).Value)
            .HasMaxLength(200);

        builder.HasKey("ContestId", "ParticipatingCountryId").IsClustered();

        builder.ToTable(AddParticipantGroupEnumCheckConstraint);

        builder.ToTable(AddValueNullabilityCheckConstraint);
    }

    private static void AddContestYearCheckConstraint(TableBuilder<Contest> builder) =>
        builder.HasCheckConstraint("ck_contest_contest_year", "[contest_year] BETWEEN 2016 AND 2050");

    private static void AddContestFormatEnumCheckConstraint(TableBuilder<Contest> builder) =>
        builder.HasCheckConstraint("ck_contest_contest_format_enum",
            $"[contest_format] IN {EnumHelpers.GetSqlNameListInParentheses<ContestFormat>()}");

    private static void AddContestStatusEnumCheckConstraint(TableBuilder<Contest> builder) =>
        builder.HasCheckConstraint("ck_contest_contest_status_enum",
            $"[contest_status] IN {EnumHelpers.GetSqlNameListInParentheses<ContestStatus>()}");

    private static void AddContestStageEnumCheckConstraint(OwnedNavigationTableBuilder<Contest, BroadcastMemo> builder) =>
        builder.HasCheckConstraint("ck_contest_broadcast_memo_contest_stage_enum",
            $"[contest_stage] IN {EnumHelpers.GetSqlNameListInParentheses<ContestStage>()}");

    private static void AddBroadcastStatusEnumCheckConstraint(OwnedNavigationTableBuilder<Contest, BroadcastMemo> builder) =>
        builder.HasCheckConstraint("ck_contest_broadcast_memo_broadcast_status_enum",
            $"[broadcast_status] IN {EnumHelpers.GetSqlNameListInParentheses<BroadcastStatus>()}");

    private static void AddParticipantGroupEnumCheckConstraint(OwnedNavigationTableBuilder<Contest, Participant> builder) =>
        builder.HasCheckConstraint("ck_contest_participant_participant_group_enum",
            $"[participant_group] IN {EnumHelpers.GetSqlIntegerListInParentheses<ParticipantGroup>()}");

    private static void AddValueNullabilityCheckConstraint(OwnedNavigationTableBuilder<Contest, Participant> builder) =>
        builder.HasCheckConstraint("ck_contest_participant_value_nullability",
            "([participant_group] = 0 AND [act_name] IS NULL AND [song_title] IS NULL) " +
            "OR ([participant_group] <> 0 AND [act_name] IS NOT NULL AND [song_title] IS NOT NULL)");
}
