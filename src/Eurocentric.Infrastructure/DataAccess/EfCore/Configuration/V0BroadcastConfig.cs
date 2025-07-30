using Eurocentric.Domain.V0Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Configuration;

internal sealed class V0BroadcastConfig : IEntityTypeConfiguration<Broadcast>
{
    public void Configure(EntityTypeBuilder<Broadcast> builder)
    {
        builder.ToTable("broadcast", "v0");

        builder.Property(broadcast => broadcast.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(broadcast => broadcast.BroadcastDate)
            .IsRequired();

        builder.Property(broadcast => broadcast.ParentContestId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(broadcast => broadcast.ContestStage)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(broadcast => broadcast.Completed)
            .IsRequired();

        builder.OwnsMany(broadcast => broadcast.Competitors, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Juries, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Televotes, ConfigureAsTable);

        builder.HasKey(broadcast => broadcast.Id).IsClustered();

        builder.HasAlternateKey(broadcast => new { broadcast.ParentContestId, broadcast.ContestStage });

        builder.HasIndex(broadcast => broadcast.BroadcastDate).IsUnique();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Competitor> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("broadcast_competitor", "v0");

        builder.WithOwner()
            .HasForeignKey("BroadcastId")
            .HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(competitor => competitor.CompetingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(competitor => competitor.FinishingPosition)
            .IsRequired();

        builder.Property(competitor => competitor.RunningOrderPosition)
            .IsRequired();

        builder.OwnsMany(competitor => competitor.JuryAwards, ConfigureAsTable);

        builder.OwnsMany(competitor => competitor.TelevoteAwards, ConfigureAsTable);

        builder.HasKey("BroadcastId", "CompetingCountryId").IsClustered();

        builder.HasIndex("BroadcastId", "RunningOrderPosition").IsUnique();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, JuryAward> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("broadcast_competitor_jury_award", "v0");

        builder.Property<int>("Id")
            .IsRequired()
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder.Property(award => award.VotingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(award => award.PointsValue)
            .HasConversion<int>()
            .IsRequired();

        builder.HasKey("Id").IsClustered();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, TelevoteAward> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("broadcast_competitor_televote_award", "v0");

        builder.Property<int>("Id")
            .IsRequired()
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder.Property(award => award.VotingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(award => award.PointsValue)
            .HasConversion<int>()
            .IsRequired();

        builder.HasKey("Id").IsClustered();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Jury> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("broadcast_jury", "v0");

        builder.WithOwner()
            .HasForeignKey("BroadcastId")
            .HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(voter => voter.VotingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(voter => voter.PointsAwarded)
            .IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Televote> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("broadcast_televote", "v0");

        builder.WithOwner()
            .HasForeignKey("BroadcastId")
            .HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(voter => voter.VotingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(voter => voter.PointsAwarded)
            .IsRequired();

        builder.HasKey("BroadcastId", "VotingCountryId").IsClustered();
    }
}
