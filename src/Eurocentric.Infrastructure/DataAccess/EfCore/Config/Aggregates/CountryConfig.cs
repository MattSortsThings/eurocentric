using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Infrastructure.DataAccess.EfCore.Config.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config.Aggregates;

internal sealed class CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("country");

        builder.Property(country => country.Id)
            .HasConversion<CountryIdConverter>()
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(country => country.CountryCode)
            .HasConversion<CountryCodeConverter>()
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired();

        builder.Property(country => country.CountryName)
            .HasConversion<CountryNameConverter>()
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

        builder.WithOwner()
            .HasForeignKey("CountryId")
            .HasPrincipalKey(country => country.Id);

        builder.Property<int>("Id")
            .IsRequired()
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.Property(contestId => contestId.Value)
            .HasColumnName("participating_contest_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.HasKey("Id").IsClustered();

        builder.HasIndex("CountryId", "Value").IsUnique();
    }
}
