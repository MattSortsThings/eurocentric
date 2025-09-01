using Eurocentric.Domain.V0.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config.V0;

internal sealed class ContestConfig : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.ToTable("contest", "v0");

        builder.Property(contest => contest.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(contest => contest.ContestYear)
            .IsRequired();

        builder.Property(contest => contest.CityName)
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
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, ChildBroadcast> builder)
    {
        builder.ToTable("contest_child_broadcast", "v0");

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner()
            .HasForeignKey("ContestId")
            .HasPrincipalKey(contest => contest.Id);

        builder.Property(broadcast => broadcast.BroadcastId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(broadcast => broadcast.ContestStage)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(broadcast => broadcast.Completed)
            .IsRequired();

        builder.HasKey("ContestId", "BroadcastId").IsClustered();

        builder.HasIndex("ContestId", "ContestStage").IsUnique();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, Participant> builder)
    {
        builder.ToTable("contest_participant", "v0");

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner()
            .HasForeignKey("ContestId")
            .HasPrincipalKey(contest => contest.Id);

        builder.Property(participant => participant.ParticipatingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(participant => participant.SemiFinalDraw)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(participant => participant.ActName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(participant => participant.SongTitle)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasKey("ContestId", "ParticipatingCountryId").IsClustered();
    }

    private static void ConfigureAsColumn(OwnedNavigationBuilder<Contest, GlobalTelevote> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.Property(televote => televote.ParticipatingCountryId)
            .HasColumnName("global_televote_participating_country_id")
            .IsRequired()
            .ValueGeneratedNever();
    }
}
