using Eurocentric.Domain.V0.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config.V0;

internal sealed class CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("country", "v0");

        builder.Property(country => country.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(country => country.CountryCode)
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired();

        builder.Property(country => country.CountryName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(country => country.ParticipatingContestIds)
            .HasMaxLength(4000)
            .IsRequired();

        builder.HasKey(country => country.Id).IsClustered();

        builder.HasAlternateKey(country => country.CountryCode);
    }
}
