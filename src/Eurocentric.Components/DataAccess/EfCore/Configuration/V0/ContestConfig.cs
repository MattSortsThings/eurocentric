using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Aggregates.V0;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Configuration.V0;

internal sealed class ContestConfig : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.ToTable("contest", DbSchemas.V0);

        builder.Property(contest => contest.Id).HasColumnName("contest_id").IsRequired().ValueGeneratedNever();

        builder.Property(contest => contest.ContestYear).HasColumnName("contest_year").IsRequired();

        builder.Property(contest => contest.CityName).HasColumnName("city_name").HasMaxLength(200).IsRequired();

        builder
            .Property(contest => contest.GrandFinalVotingRules)
            .HasColumnName("grand_final_voting_rules")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder
            .Property(contest => contest.SemiFinalVotingRules)
            .HasColumnName("semi_final_voting_rules")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(contest => contest.Queryable).HasColumnName("queryable").IsRequired();

        builder.OwnsOne(contest => contest.GlobalTelevote, ConfigureAsColumn);

        builder.OwnsMany(contest => contest.ChildBroadcasts, ConfigureAsTable);

        builder.OwnsMany(contest => contest.Participants, ConfigureAsTable);

        builder.HasKey(contest => contest.Id).IsClustered();

        builder.HasIndex(contest => contest.ContestYear).IsUnique();

        builder.ToTable(table =>
            table.HasCheckConstraint("CK_contest_contest_year", "[contest_year] BETWEEN 2016 and 2050")
        );
    }

    private static void ConfigureAsColumn(OwnedNavigationBuilder<Contest, GlobalTelevote> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder
            .Property(televote => televote.VotingCountryId)
            .HasColumnName("global_televote_voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, ChildBroadcast> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("contest_child_broadcast", DbSchemas.V0);

        builder.Property<Guid>("ContestId").HasColumnName("contest_id").IsRequired().ValueGeneratedNever();

        builder.WithOwner().HasForeignKey("ContestId").HasPrincipalKey(contest => contest.Id);

        builder
            .Property(broadcast => broadcast.ChildBroadcastId)
            .HasColumnName("child_broadcast_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(broadcast => broadcast.ContestStage)
            .HasColumnName("contest_stage")
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(broadcast => broadcast.Completed).HasColumnName("completed").IsRequired();

        builder.HasKey("ContestId", "ChildBroadcastId").IsClustered();

        builder.HasIndex("ContestId", "ContestStage").IsUnique();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, Participant> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("contest_participant", DbSchemas.V0);

        builder.Property<Guid>("ContestId").HasColumnName("contest_id").IsRequired().ValueGeneratedNever();

        builder.WithOwner().HasForeignKey("ContestId").HasPrincipalKey(contest => contest.Id);

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
            .IsRequired();

        builder.Property(participant => participant.ActName).HasColumnName("act_name").HasMaxLength(200).IsRequired();

        builder
            .Property(participant => participant.SongTitle)
            .HasColumnName("song_title")
            .HasMaxLength(200)
            .IsRequired();

        builder.HasKey("ContestId", "ParticipatingCountryId").IsClustered();
    }
}
