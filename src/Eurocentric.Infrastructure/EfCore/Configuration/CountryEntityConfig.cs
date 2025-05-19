using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.EfCore.Configuration;

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

        builder.Property(country => country.Name)
            .HasColumnName("country_name")
            .HasConversion(src => src.Value, value => CountryName.FromValue(value).Value)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsMany(country => country.ContestMemos, ConfigureAsTable);

        builder.HasKey(country => country.Id).IsClustered();

        builder.HasAlternateKey(country => country.CountryCode);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Country, ContestMemo> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.CountryContestMemo);

        builder.Property<int>("Id")
            .IsRequired()
            .UseIdentityColumn(1)
            .ValueGeneratedOnAdd();

        builder.WithOwner()
            .HasForeignKey("CountryId")
            .HasPrincipalKey(country => country.Id);

        builder.Property(contestMemo => contestMemo.ContestId)
            .HasConversion(src => src.Value, value => ContestId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(contestMemo => contestMemo.Status)
            .HasColumnName("contest_status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasKey("Id");

        builder.HasIndex("CountryId", "ContestId").IsUnique();

        builder.ToTable(AddContestStatusEnumCheckConstraint);
    }

    private static void AddContestStatusEnumCheckConstraint(OwnedNavigationTableBuilder<Country, ContestMemo> builder) =>
        builder.HasCheckConstraint("ck_country_contest_memo_contest_status_enum",
            $"[contest_status] IN {EnumHelpers.GetSqlNameListInParentheses<ContestStatus>()}");
}
