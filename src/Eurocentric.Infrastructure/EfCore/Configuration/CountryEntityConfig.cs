using Eurocentric.Domain.Countries;
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
            .IsRequired()
            .ValueGeneratedNever()
            .HasConversion(src => src.Value, value => CountryId.FromValue(value));

        builder.ComplexProperty(country => country.CountryCode, ConfigureAsColumn);

        builder.ComplexProperty(country => country.Name, ConfigureAsColumn);

        builder.OwnsMany(country => country.ContestMemos, ConfigureAsTable);

        builder.HasKey(country => country.Id).IsClustered();
    }

    private static void ConfigureAsColumn(ComplexPropertyBuilder<CountryCode> builder)
    {
        builder.IsRequired();

        builder.Property(countryCode => countryCode.Value)
            .IsRequired()
            .HasColumnName("country_code")
            .HasColumnType("nchar(2)");
    }

    private static void ConfigureAsColumn(ComplexPropertyBuilder<CountryName> builder)
    {
        builder.IsRequired();

        builder.Property(countryName => countryName.Value)
            .IsRequired()
            .HasColumnName("name")
            .HasColumnType("nvarchar(200)");
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Country, ContestMemo> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(DbConstants.TableNames.CountryContestMemo);

        builder.Property<int>("Id")
            .IsRequired()
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(1);

        builder.WithOwner()
            .HasForeignKey("CountryId")
            .HasPrincipalKey(country => country.Id);

        builder.Property(contestMemo => contestMemo.ContestId)
            .IsRequired()
            .ValueGeneratedNever()
            .HasConversion(src => src.Value, value => ContestId.FromValue(value));

        builder.Property(contestMemo => contestMemo.Status)
            .IsRequired()
            .HasColumnType("nvarchar(20)")
            .HasConversion<string>();

        builder.HasKey("Id");

        builder.HasIndex("CountryId", "ContestId").IsUnique();
    }
}
