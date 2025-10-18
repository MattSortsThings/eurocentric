using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.V0.Aggregates.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Config.V0;

internal sealed class CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable(Tables.V0.Country, Schemas.V0);

        builder.Property(country => country.Id).HasColumnName("country_id").IsRequired().ValueGeneratedNever();

        builder.Property(country => country.CountryCode).HasMaxLength(2).IsFixedLength().IsRequired();

        builder.Property(country => country.CountryName).HasMaxLength(200).IsRequired();

        builder.OwnsMany(country => country.ContestRoles, ConfigureAsTable);

        builder.HasKey(country => country.Id).IsClustered();

        builder.HasAlternateKey(country => country.CountryCode);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Country, ContestRole> builder)
    {
        builder.ToTable(Tables.V0.CountryContestRole, Schemas.V0);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner().HasForeignKey("CountryId").HasPrincipalKey(country => country.Id);

        builder.Property<int>("RowId").IsRequired().UseIdentityColumn().ValueGeneratedOnAdd();

        builder.Property(role => role.ContestId).IsRequired().ValueGeneratedNever();

        builder.Property(role => role.ContestRoleType).HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.HasKey("RowId").IsClustered();

        builder.HasIndex("CountryId", "ContestId").IsUnique();
    }
}
