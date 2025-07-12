using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Configuration;

internal sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("country");

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

        builder.OwnsMany(country => country.ParticipatingContestIds, ConfigureAsTable);

        builder.HasKey(country => country.Id).IsClustered();

        builder.HasAlternateKey(country => country.CountryCode);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Country, ContestId> builder)
    {
        builder.ToTable("country_participating_contest");

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property<int>("Id")
            .IsRequired()
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.Property(contestId => contestId.Value)
            .HasColumnName("participating_contest_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.WithOwner()
            .HasForeignKey("CountryId")
            .HasPrincipalKey(country => country.Id);

        builder.HasKey("Id");

        builder.HasIndex("CountryId", "Value").IsUnique();
    }
}
