using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Aggregates.V0.Contests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Configuration.V0;

internal class ContestConfig : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.ToTable("contest", V0SchemaConstants.SchemaName);

        builder.Property(contest => contest.Id).HasColumnName("contest_id").IsRequired().ValueGeneratedNever();

        builder.Property(contest => contest.ContestYear).HasColumnName("contest_year").IsRequired();

        builder
            .Property(contest => contest.CityName)
            .HasColumnName("city_name")
            .HasMaxLength(150)
            .IsRequired()
            .IsUnicode();

        builder
            .Property(contest => contest.SemiFinalBroadcastFormat)
            .HasColumnName("semi_final_broadcast_format")
            .HasConversion<string>()
            .HasMaxLength(15)
            .IsRequired()
            .IsUnicode(false);

        builder
            .Property(contest => contest.GrandFinalBroadcastFormat)
            .HasColumnName("grand_final_broadcast_format")
            .HasConversion<string>()
            .HasMaxLength(15)
            .IsRequired()
            .IsUnicode(false);

        builder.Property(contest => contest.Queryable).HasColumnName("queryable").IsRequired();

        builder
            .OwnsOne(contest => contest.GlobalTelevote, ConfigureAsColumn)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder
            .OwnsMany(contest => contest.ChildBroadcasts, ConfigureAsTable)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder
            .OwnsMany(contest => contest.Participants, ConfigureAsTable)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.HasKey(contest => contest.Id).IsClustered();

        builder.HasAlternateKey(contest => contest.ContestYear);

        builder.ToTable(AddContestYearCheckConstraint);
    }

    private static void AddContestYearCheckConstraint(TableBuilder<Contest> builder) =>
        builder.HasCheckConstraint("CK_contest_contest_year", "contest_year BETWEEN 2016 AND 2030");

    private static void ConfigureAsColumn(OwnedNavigationBuilder<Contest, GlobalTelevote> builder)
    {
        builder
            .Property(globalTelevote => globalTelevote.VotingCountryId)
            .HasColumnName("global_televote_voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, ChildBroadcast> builder)
    {
        builder.ToTable("contest_child_broadcast", V0SchemaConstants.SchemaName);

        builder
            .Property<Guid>(ShadowPropertyNames.ContestId)
            .HasColumnName("contest_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(childBroadcast => childBroadcast.ChildBroadcastId)
            .HasColumnName("child_broadcast_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(childBroadcast => childBroadcast.ContestStage)
            .HasColumnName("contest_stage")
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired()
            .IsUnicode(false);

        builder.Property(childBroadcast => childBroadcast.Completed).HasColumnName("completed").IsRequired();

        builder.HasKey(ShadowPropertyNames.ContestId, nameof(ChildBroadcast.ChildBroadcastId)).IsClustered();

        builder.HasIndex(ShadowPropertyNames.ContestId, nameof(ChildBroadcast.ContestStage)).IsUnique();

        builder.WithOwner().HasForeignKey(ShadowPropertyNames.ContestId).HasPrincipalKey(contest => contest.Id);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, Participant> builder)
    {
        builder.ToTable("contest_participant", V0SchemaConstants.SchemaName);

        builder
            .Property<Guid>(ShadowPropertyNames.ContestId)
            .HasColumnName("contest_id")
            .IsRequired()
            .ValueGeneratedNever();

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

        builder.HasKey(ShadowPropertyNames.ContestId, nameof(Participant.ParticipatingCountryId)).IsClustered();

        builder.WithOwner().HasForeignKey(ShadowPropertyNames.ContestId).HasPrincipalKey(contest => contest.Id);
    }

    private static class ShadowPropertyNames
    {
        public const string ContestId = "ContestId";
    }
}
