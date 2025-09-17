using Eurocentric.Domain.Enums;
using Eurocentric.Domain.V0Entities;
using Eurocentric.Infrastructure.DataAccess.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config;

internal sealed class V0ContestConfig : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.ToTable(V0Schema.Tables.Contest, V0Schema.Name);

        builder.Property(contest => contest.Id)
            .HasColumnName("contest_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(contest => contest.ContestYear)
            .IsRequired();

        builder.Property(contest => contest.CityName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(contest => contest.ContestRules)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(contest => contest.Queryable)
            .IsRequired();

        builder.OwnsMany(contest => contest.ChildBroadcasts, ConfigureAsTable);

        builder.OwnsMany(contest => contest.Participants, ConfigureAsTable);

        builder.OwnsOne(contest => contest.GlobalTelevote, ConfigureAsColumn);

        builder.HasKey(contest => contest.Id).IsClustered();

        builder.HasAlternateKey(contest => contest.ContestYear);

        builder.HasDiscriminator(contest => contest.ContestRules)
            .HasValue<LiverpoolRulesContest>(ContestRules.Liverpool)
            .HasValue<StockholmRulesContest>(ContestRules.Stockholm)
            .IsComplete();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, ChildBroadcast> builder)
    {
        builder.ToTable(V0Schema.Tables.ChildBroadcast, V0Schema.Name);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner()
            .HasForeignKey("ContestId")
            .HasPrincipalKey(contest => contest.Id);

        builder.Property(childBroadcast => childBroadcast.ChildBroadcastId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(childBroadcast => childBroadcast.ContestStage)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(childBroadcast => childBroadcast.Completed)
            .IsRequired();

        builder.HasKey("ContestId", "ChildBroadcastId").IsClustered();

        builder.HasIndex("ContestId", "ContestStage").IsUnique();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, Participant> builder)
    {
        builder.ToTable(V0Schema.Tables.Participant, V0Schema.Name);

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

        builder.Property(televote => televote.VotingCountryId)
            .HasColumnName("global_televote_voting_country_id")
            .IsRequired()
            .ValueGeneratedNever();
    }
}
