using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Aggregates.Placeholders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Configuration;

internal sealed class ContestConfig : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.ToTable(DbTables.Placeholder.Contest, DbSchemas.Placeholder);

        builder.Property(contest => contest.Id).HasColumnName("contest_id").IsRequired().ValueGeneratedNever();

        builder.Property(contest => contest.ContestYear).HasColumnName("contest_year").IsRequired();

        builder
            .Property(contest => contest.CityName)
            .HasColumnName("city_name")
            .HasMaxLength(200)
            .IsRequired()
            .IsUnicode();

        builder
            .Property(contest => contest.SemiFinalVotingRules)
            .HasColumnName("semi_final_voting_rules")
            .HasConversion<string>()
            .HasMaxLength(15)
            .IsRequired()
            .IsUnicode(false);

        builder
            .Property(contest => contest.GrandFinalVotingRules)
            .HasColumnName("grand_final_voting_rules")
            .HasConversion<string>()
            .HasMaxLength(15)
            .IsRequired()
            .IsUnicode(false);

        builder.Property(contest => contest.Queryable).HasColumnName("queryable").IsRequired();

        builder.OwnsOne(contest => contest.GlobalTelevote, ConfigureAsColumn);

        builder.OwnsMany(contest => contest.BroadcastMemos, ConfigureAsTable);

        builder.OwnsMany(contest => contest.Participants, ConfigureAsTable);

        builder.HasKey(contest => contest.Id).IsClustered();

        builder.HasAlternateKey(contest => contest.ContestYear);

        builder.ToTable(table =>
            table.HasCheckConstraint("CK_contest_contest_year", "[contest_year] BETWEEN 2016 AND 2050")
        );
    }

    private static void ConfigureAsColumn(OwnedNavigationBuilder<Contest, GlobalTelevote> builder)
    {
        builder
            .Property(globalTelevote => globalTelevote.VotingCountryId)
            .HasColumnName("global_televote_voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, BroadcastMemo> builder)
    {
        builder.ToTable(DbTables.Placeholder.ContestBroadcastMemo, DbSchemas.Placeholder);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

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

        builder.WithOwner().HasForeignKey("ContestId").HasPrincipalKey(contest => contest.Id);

        builder.HasKey("RowId").IsClustered();

        builder.HasIndex("ContestId", "ContestStage").IsUnique();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, Participant> builder)
    {
        builder.ToTable(DbTables.Placeholder.ContestParticipant, DbSchemas.Placeholder);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

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
            .HasMaxLength(200)
            .IsRequired()
            .IsUnicode();

        builder
            .Property(participant => participant.SongTitle)
            .HasColumnName("song_title")
            .HasMaxLength(200)
            .IsRequired()
            .IsUnicode();

        builder.WithOwner().HasForeignKey("ContestId").HasPrincipalKey(contest => contest.Id);

        builder.HasKey("ContestId", "ParticipatingCountryId").IsClustered();
    }
}
