using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Config;

internal sealed class ContestConfig : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.ToTable(Tables.Dbo.Contest, Schemas.Dbo);

        builder
            .HasDiscriminator(contest => contest.ContestRules)
            .HasValue<LiverpoolRulesContest>(ContestRules.Liverpool)
            .HasValue<StockholmRulesContest>(ContestRules.Stockholm)
            .IsComplete();

        builder
            .Property(contest => contest.Id)
            .HasColumnName("contest_id")
            .HasConversion(src => src.Value, value => ContestId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(contest => contest.ContestYear)
            .HasConversion(src => src.Value, value => ContestYear.FromValue(value).Value)
            .IsRequired();

        builder
            .Property(contest => contest.CityName)
            .HasConversion(src => src.Value, value => CityName.FromValue(value).Value)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(contest => contest.ContestRules).HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.Property(contest => contest.Queryable).IsRequired();

        builder.OwnsMany(contest => contest.ChildBroadcasts, ConfigureAsTable);

        builder.Ignore(contest => contest.GlobalTelevote);

        builder.OwnsOne(contest => contest.GlobalTelevote, ConfigureAsColumn);

        builder.OwnsMany(contest => contest.Participants, ConfigureAsTable);

        builder.HasKey(contest => contest.Id).IsClustered();

        builder.HasAlternateKey(contest => contest.ContestYear);

        builder.ToTable(AddContestYearCheckConstraint);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, ChildBroadcast> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(Tables.Dbo.ContestChildBroadcast, Schemas.Dbo);

        builder.WithOwner().HasForeignKey("ContestId").HasPrincipalKey(contest => contest.Id);

        builder
            .Property(broadcast => broadcast.ChildBroadcastId)
            .HasConversion(src => src.Value, value => BroadcastId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(broadcast => broadcast.ContestStage).HasConversion<string>().HasMaxLength(10).IsRequired();

        builder.Property(broadcast => broadcast.Completed).IsRequired();

        builder.HasKey("ContestId", "ChildBroadcastId").IsClustered();

        builder.HasIndex("ContestId", "ContestStage").IsUnique();
    }

    private static void ConfigureAsColumn(OwnedNavigationBuilder<Contest, GlobalTelevote> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder
            .Property(televote => televote.VotingCountryId)
            .HasColumnName("global_televote_voting_country_id")
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Contest, Participant> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(Tables.Dbo.ContestParticipant, Schemas.Dbo);

        builder.WithOwner().HasForeignKey("ContestId").HasPrincipalKey(contest => contest.Id);

        builder
            .Property(participant => participant.ParticipatingCountryId)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(participant => participant.SemiFinalDraw)
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

        builder
            .Property(participant => participant.ActName)
            .HasConversion(src => src.Value, value => ActName.FromValue(value).Value)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(participant => participant.SongTitle)
            .HasConversion(src => src.Value, value => SongTitle.FromValue(value).Value)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasKey("ContestId", "ParticipatingCountryId").IsClustered();
    }

    private static void AddContestYearCheckConstraint(TableBuilder<Contest> builder) =>
        builder.HasCheckConstraint("ck_contest_contest_year", "contest_year BETWEEN 2016 AND 2050");
}
