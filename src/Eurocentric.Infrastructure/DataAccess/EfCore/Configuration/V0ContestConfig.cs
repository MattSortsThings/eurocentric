using Eurocentric.Domain.V0Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Configuration;

internal sealed class V0ContestConfig : IEntityTypeConfiguration<Contest>
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

        builder.HasKey(contest => contest.Id).IsClustered();

        builder.HasAlternateKey(contest => contest.ContestYear);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, ChildBroadcast> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("contest_child_broadcast", "v0");

        builder.WithOwner()
            .HasForeignKey("ContestId")
            .HasPrincipalKey(contest => contest.Id);

        builder.Property(memo => memo.BroadcastId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(memo => memo.ContestStage)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(memo => memo.Completed)
            .IsRequired();

        builder.HasKey("ContestId", "BroadcastId").IsClustered();

        builder.HasIndex("ContestId", "ContestStage").IsUnique();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, Participant> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("contest_participant", "v0");

        builder.WithOwner()
            .HasForeignKey("ContestId")
            .HasPrincipalKey(contest => contest.Id);

        builder.Property(participant => participant.ParticipatingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(participant => participant.ParticipantGroup)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(participant => participant.ActName)
            .HasMaxLength(200);

        builder.Property(participant => participant.SongTitle)
            .HasMaxLength(200);

        builder.HasKey("ContestId", "ParticipatingCountryId").IsClustered();
    }
}
