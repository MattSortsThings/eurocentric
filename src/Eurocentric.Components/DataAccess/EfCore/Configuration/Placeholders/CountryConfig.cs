using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Placeholders.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Configuration.Placeholders;

internal sealed class CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("country", DbSchemas.Placeholder);

        builder.Property(country => country.Id).HasColumnName("country_id").IsRequired().ValueGeneratedNever();

        builder
            .Property(country => country.CountryCode)
            .HasColumnName("country_code")
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired()
            .IsUnicode(false);

        builder
            .Property(country => country.CountryName)
            .HasColumnName("country_name")
            .HasMaxLength(150)
            .IsRequired()
            .IsUnicode();

        builder
            .Property(country => country.CountryType)
            .HasColumnName("country_type")
            .HasConversion<string>()
            .HasMaxLength(6)
            .IsUnicode(false);

        builder
            .PrimitiveCollection(country => country.ContestIds)
            .HasColumnName("contest_ids")
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.HasKey(country => country.Id).IsClustered();

        builder.HasAlternateKey(country => country.CountryCode);
    }
}
