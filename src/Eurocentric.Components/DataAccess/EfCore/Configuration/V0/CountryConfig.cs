using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Aggregates.V0;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Configuration.V0;

internal sealed class CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("country", DbSchemas.V0);

        builder.Property(country => country.Id).HasColumnName("country_id").IsRequired().ValueGeneratedNever();

        builder.Property(country => country.CountryCode).HasColumnName("country_code").HasMaxLength(2).IsRequired();

        builder.Property(country => country.CountryName).HasColumnName("country_name").HasMaxLength(200).IsRequired();

        builder.OwnsMany(country => country.ContestRoles, ConfigureAsTable);

        builder.HasKey(country => country.Id).IsClustered();

        builder.HasIndex(country => country.CountryCode).IsUnique();
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Country, ContestRole> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("country_contest_role", DbSchemas.V0);

        builder.WithOwner().HasForeignKey("CountryId").HasPrincipalKey(country => country.Id);

        builder.Property<int>("RowId").HasColumnName("row_id").IsRequired().UseIdentityColumn().ValueGeneratedOnAdd();

        builder.Property<Guid>("CountryId").HasColumnName("country_id").IsRequired().ValueGeneratedNever();

        builder.Property(role => role.ContestId).HasColumnName("contest_id").IsRequired().ValueGeneratedNever();

        builder
            .Property(role => role.ContestRoleType)
            .HasColumnName("contest_role_type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasKey("RowId").IsClustered();

        builder.HasIndex("CountryId", "ContestId").IsUnique();
    }
}
