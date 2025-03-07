using Eurocentric.Domain.Countries;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.DataAccess.EfCore.Configuration.Entities;

internal sealed class CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> country)
    {
        country.ToTable("country");

        country.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever()
            .HasConversion(src => src.Value, value => CountryId.FromValue(value));

        country.Property(c => c.CountryCode)
            .IsRequired()
            .HasMaxLength(CountryCode.RequiredLengthInChars)
            .IsFixedLength()
            .HasConversion(src => src.Value, value => CountryCode.FromValue(value));

        country.Property(c => c.CountryName)
            .IsRequired()
            .HasMaxLength(CountryName.MaxLengthInChars)
            .HasConversion(src => src.Value, value => CountryName.FromValue(value));

        country.Property(c => c.CountryType)
            .IsRequired()
            .HasConversion<int>();

        country.OwnsMany(c => c.ContestIds, ConfigureAsTable)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        country.HasKey(c => c.Id);

        country.HasAlternateKey(c => c.CountryCode);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Country, ContestId> contestId)
    {
        contestId.ToTable("country_contest");

        contestId.Property<int>("Id")
            .IsRequired()
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(1);

        contestId.WithOwner().HasForeignKey("CountryId").HasPrincipalKey(c => c.Id);

        contestId.Property(id => id.Value)
            .HasColumnName("contest_id")
            .IsRequired()
            .ValueGeneratedNever();

        contestId.HasKey("Id");

        contestId.HasIndex("CountryId", "Value").IsUnique();
    }
}
