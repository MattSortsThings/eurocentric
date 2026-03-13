using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Aggregates.V0.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Configuration.V0;

internal sealed class CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("country", V0SchemaConstants.SchemaName);

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
            .IsRequired()
            .IsUnicode(false);

        builder
            .PrimitiveCollection(country => country.ActiveContestIds)
            .HasColumnName("active_contest_ids")
            .HasColumnType("nvarchar(max)")
            .IsRequired()
            .IsUnicode();

        builder.HasKey(country => country.Id).IsClustered();

        builder.HasAlternateKey(country => country.CountryCode);
    }
}
