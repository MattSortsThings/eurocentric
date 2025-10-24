using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Config;

internal sealed class CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable(Tables.Dbo.Country, Schemas.Dbo);

        builder
            .Property(country => country.Id)
            .HasColumnName("country_id")
            .HasConversion(src => src.Value, value => CountryId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(country => country.CountryCode)
            .HasConversion(src => src.Value, value => CountryCode.FromValue(value).Value)
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired();

        builder
            .Property(country => country.CountryName)
            .HasConversion(src => src.Value, value => CountryName.FromValue(value).Value)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsMany(country => country.ContestRoles, ConfigureAsTable);

        builder.HasKey(country => country.Id).IsClustered();

        builder.HasAlternateKey(country => country.CountryCode);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Country, ContestRole> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable(Tables.Dbo.CountryContestRole, Schemas.Dbo);

        builder.WithOwner().HasForeignKey("CountryId").HasPrincipalKey(country => country.Id);

        builder.Property<int>("RowId").IsRequired().UseIdentityColumn().ValueGeneratedOnAdd();

        builder
            .Property(role => role.ContestId)
            .HasConversion(src => src.Value, value => ContestId.FromValue(value))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(role => role.ContestRoleType).HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.HasKey("RowId").IsClustered();

        builder.HasIndex("CountryId", "ContestId").IsUnique();
    }
}
