using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Placeholders.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Configuration.Placeholders;

internal sealed class ContestConfig : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.ToTable("contest", DbSchemas.Placeholder);

        builder.Property(contest => contest.Id).HasColumnName("contest_id").IsRequired().ValueGeneratedNever();

        builder.Property(contest => contest.ContestYear).HasColumnName("contest_year").IsRequired();

        builder
            .Property(contest => contest.CityName)
            .HasColumnName("city_name")
            .HasMaxLength(150)
            .IsRequired()
            .IsUnicode();

        builder
            .Property(contest => contest.SemiFinalVotingFormat)
            .HasColumnName("semi_final_voting_format")
            .HasConversion<string>()
            .HasMaxLength(15)
            .IsRequired()
            .IsUnicode(false);

        builder
            .Property(contest => contest.GrandFinalVotingFormat)
            .HasColumnName("grand_final_voting_format")
            .HasConversion<string>()
            .HasMaxLength(15)
            .IsRequired()
            .IsUnicode(false);

        builder.Property(contest => contest.Queryable).HasColumnName("queryable").IsRequired();

        builder.OwnsMany(contest => contest.BroadcastMemos, ConfigureAsTable);

        builder.OwnsOne(contest => contest.GlobalTelevote, ConfigureAsColumn);

        builder.OwnsMany(contest => contest.Participants, ConfigureAsTable);

        builder.HasKey(contest => contest.Id).IsClustered();

        builder.HasAlternateKey(contest => contest.ContestYear);

        builder.ToTable(table =>
            table.HasCheckConstraint("CK_contest_contest_year", "[contest_year] BETWEEN 2016 AND 2050")
        );
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, BroadcastMemo> builder)
    {
        builder.ToTable("contest_broadcast_memo", DbSchemas.Placeholder);

        builder.Property<int>("RowId").HasColumnName("row_id").IsRequired().UseIdentityColumn().ValueGeneratedOnAdd();

        builder.Property<Guid>("ContestId").HasColumnName("contest_id").IsRequired().ValueGeneratedNever();

        builder
            .Property(broadcastMemo => broadcastMemo.BroadcastId)
            .HasColumnName("broadcast_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(broadcastMemo => broadcastMemo.ContestStage)
            .HasColumnName("contest_stage")
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired()
            .IsUnicode(false);

        builder.Property(broadcastMemo => broadcastMemo.Completed).HasColumnName("completed").IsRequired();

        builder.HasKey("RowId").IsClustered();

        builder.HasIndex("ContestId", "BroadcastId").IsUnique();

        builder.WithOwner().HasForeignKey("ContestId").HasPrincipalKey(contest => contest.Id);
    }

    private static void ConfigureAsColumn(OwnedNavigationBuilder<Contest, GlobalTelevote> builder)
    {
        builder
            .Property(globalTelevote => globalTelevote.VotingCountryId)
            .HasColumnName("global_televote_voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, Participant> builder)
    {
        builder.ToTable("contest_participant", DbSchemas.Placeholder);

        builder.Property<Guid>("ContestId").HasColumnName("contest_id").IsRequired().ValueGeneratedNever();

        builder
            .Property(participant => participant.ParticipatingCountryId)
            .HasColumnName("participating_country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(participant => participant.SemiFinalDraw)
            .HasColumnName("semi_final_draw")
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired()
            .IsUnicode(false);

        builder
            .Property(participant => participant.ActName)
            .HasColumnName("act_name")
            .HasMaxLength(150)
            .IsRequired()
            .IsUnicode();

        builder
            .Property(participant => participant.SongTitle)
            .HasColumnName("song_title")
            .HasMaxLength(150)
            .IsRequired()
            .IsUnicode();

        builder.HasKey("ContestId", "ParticipatingCountryId").IsClustered();

        builder.WithOwner().HasForeignKey("ContestId").HasPrincipalKey(contest => contest.Id);
    }
}
