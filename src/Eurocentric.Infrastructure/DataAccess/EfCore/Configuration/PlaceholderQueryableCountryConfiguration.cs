using Eurocentric.Domain.PlaceholderEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Configuration;

internal sealed class PlaceholderQueryableCountryConfiguration : IEntityTypeConfiguration<QueryableCountry>
{
    public void Configure(EntityTypeBuilder<QueryableCountry> builder)
    {
        builder.ToTable("placeholder_queryable_country");

        builder.Property(country => country.CountryCode)
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired();

        builder.Property(country => country.CountryName)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasKey(country => country.CountryCode);
    }
}
