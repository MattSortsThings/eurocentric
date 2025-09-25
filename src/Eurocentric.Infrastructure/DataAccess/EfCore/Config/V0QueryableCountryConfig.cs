using Eurocentric.Domain.V0.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config;

internal sealed class V0QueryableCountryConfig : IEntityTypeConfiguration<QueryableCountry>
{
    public void Configure(EntityTypeBuilder<QueryableCountry> builder)
    {
        builder.ToView("vw_queryable_country", "v0");

        builder
            .Property(country => country.CountryCode)
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired();

        builder.Property(country => country.CountryName).HasMaxLength(200).IsRequired();

        builder.HasNoKey();
    }
}
