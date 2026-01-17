using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Domain.Aggregates.Placeholders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Components.DataAccess.EfCore.Configuration;

internal sealed class CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable(DbTables.Placeholder.Country, DbSchemas.Placeholder);

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
            .HasMaxLength(200)
            .IsRequired()
            .IsUnicode();

        builder
            .Property(country => country.CountryType)
            .HasColumnName("country_type")
            .HasConversion<string>()
            .HasMaxLength(6)
            .IsRequired()
            .IsUnicode(false);

        builder.OwnsMany(country => country.ContestRoles, ConfigureAsTable);

        builder.HasKey(country => country.Id).IsClustered();

        builder.HasAlternateKey(country => country.CountryCode);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Country, ContestRole> builder)
    {
        builder.ToTable(DbTables.Placeholder.CountryContestRole, DbSchemas.Placeholder);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.Property<int>("RowId").HasColumnName("row_id").IsRequired().UseIdentityColumn().ValueGeneratedOnAdd();

        builder.Property<Guid>("CountryId").HasColumnName("country_id").IsRequired().ValueGeneratedNever();

        builder
            .Property(contestRole => contestRole.ContestId)
            .HasColumnName("contest_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(contestRole => contestRole.ContestRoleType)
            .HasColumnName("contest_role_type")
            .HasConversion<string>()
            .HasMaxLength(14)
            .IsRequired()
            .IsUnicode(false);

        builder.WithOwner().HasPrincipalKey(country => country.Id).HasForeignKey("CountryId");

        builder.HasKey("RowId");

        builder.HasIndex("ContestId", "CountryId").IsUnique();
    }
}
