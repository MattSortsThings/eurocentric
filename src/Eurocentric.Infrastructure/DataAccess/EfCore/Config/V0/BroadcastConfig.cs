using Eurocentric.Domain.V0.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config.V0;

internal sealed class BroadcastConfig : IEntityTypeConfiguration<Broadcast>
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
            .IsRequired()
            .HasConversion<int>();

        builder.Property(broadcast => broadcast.Completed)
            .IsRequired();

        builder.OwnsMany(broadcast => broadcast.Competitors, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Juries, ConfigureAsTable);

        builder.OwnsMany(broadcast => broadcast.Televotes, ConfigureAsTable);

        builder.HasKey(broadcast => broadcast.Id).IsClustered();

        builder.HasAlternateKey(broadcast => broadcast.BroadcastDate);

        builder.HasAlternateKey(broadcast => new { broadcast.ParentContestId, broadcast.ContestStage });
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Competitor> builder)
    {
        builder.ToTable("broadcast_competitor", "v0");

        builder.WithOwner()
            .HasForeignKey("BroadcastId")
            .HasPrincipalKey(broadcast => broadcast.Id);

        builder.Property(competitor => competitor.CompetingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(competitor => competitor.RunningOrderPosition)
            .IsRequired();

        builder.Property(competitor => competitor.FinishingPosition)
            .IsRequired();

        builder.OwnsMany(competitor => competitor.JuryAwards, ConfigureAsTable);

        builder.OwnsMany(competitor => competitor.TelevoteAwards, ConfigureAsTable);

        builder.HasKey("BroadcastId", "CompetingCountryId").IsClustered();

        builder.HasIndex("BroadcastId", "RunningOrderPosition").IsUnique();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, JuryAward> builder)
    {
        builder.ToTable("broadcast_competitor_jury_award", "v0");

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder.Property<int>("Id")
            .IsRequired()
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(award => award.VotingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(award => award.PointsValue)
            .IsRequired()
            .HasConversion<int>();

        builder.HasKey("Id").IsClustered();

        builder.HasIndex("BroadcastId", "CompetingCountryId", "VotingCountryId").IsUnique();

        builder.HasIndex("BroadcastId");

        builder.HasIndex("CompetingCountryId");

        builder.HasIndex(award => award.VotingCountryId);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Competitor, TelevoteAward> builder)
    {
        builder.ToTable("broadcast_competitor_televote_award", "v0");

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner()
            .HasForeignKey("BroadcastId", "CompetingCountryId")
            .HasPrincipalKey("BroadcastId", "CompetingCountryId");

        builder.Property<int>("Id")
            .IsRequired()
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(award => award.VotingCountryId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(award => award.PointsValue)
            .IsRequired()
            .HasConversion<int>();

        builder.HasKey("Id").IsClustered();

        builder.HasIndex("BroadcastId", "CompetingCountryId", "VotingCountryId").IsUnique();

        builder.HasIndex("BroadcastId");

        builder.HasIndex("CompetingCountryId");

        builder.HasIndex(award => award.VotingCountryId);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Broadcast, Jury> builder)
    {
        builder.ToTable("broadcast_jury", "v0");

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

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
        builder.ToTable("broadcast_televote", "v0");

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

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
