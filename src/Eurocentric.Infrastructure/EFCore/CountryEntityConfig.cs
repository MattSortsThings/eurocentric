using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.EFCore;

internal sealed class CountryEntityConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable(DbConstants.TableNames.Country);

        builder.Property(country => country.Id)
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(country => country.CountryCode)
            .HasConversion(src => src.Value, value => CountryCode.FromValue(value).Value)
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired();

        builder.Property(country => country.CountryName)
            .HasConversion(src => src.Value, value => CountryName.FromValue(value).Value)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasKey(country => country.Id).IsClustered();

        builder.HasAlternateKey(country => country.CountryCode);

        builder.OwnsMany(country => country.ParticipatingContests, ConfigureAsTable);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Country, ContestMemo> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.CountryParticipatingContest);

        builder.Property<int>("Id")
            .UseIdentityColumn(1)
            .ValueGeneratedOnAdd();

        builder.Property(memo => memo.ContestId)
            .HasConversion(src => src.Value, value => ContestId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(memo => memo.ContestStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.WithOwner()
            .HasForeignKey("CountryId")
            .HasPrincipalKey(country => country.Id);

        builder.HasKey("Id").IsClustered();

        builder.HasIndex("CountryId", "ContestId").IsUnique();

        builder.ToTable(AddContestStatusEnumCheckConstraint);
    }

    private static void AddContestStatusEnumCheckConstraint(OwnedNavigationTableBuilder<Country, ContestMemo> builder) =>
        builder.HasCheckConstraint("ck_country_participating_contest_contest_status_enum",
            $"[contest_status] IN {EnumHelpers.GetSqlIntegerListInParentheses<ContestStatus>()}");
}
