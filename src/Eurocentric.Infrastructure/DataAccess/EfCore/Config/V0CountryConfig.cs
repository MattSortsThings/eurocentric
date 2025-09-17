using Eurocentric.Domain.V0Entities;
using Eurocentric.Infrastructure.DataAccess.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config;

internal sealed class V0CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable(V0Schema.Tables.Country, V0Schema.Name);

        builder.Property(country => country.Id)
            .HasColumnName("country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(country => country.CountryCode)
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired();

        builder.Property(country => country.CountryName)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsMany(country => country.ContestRoles, ConfigureAsTable);

        builder.HasKey(country => country.Id).IsClustered();

        builder.HasAlternateKey(country => country.CountryCode);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Country, ContestRole> builder)
    {
        builder.ToTable(V0Schema.Tables.ContestRole, V0Schema.Name);

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner()
            .HasForeignKey("CountryId")
            .HasPrincipalKey(country => country.Id);

        builder.Property<int>("RowId")
            .IsRequired()
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.Property(contestRole => contestRole.ContestId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(contestRole => contestRole.ContestRoleType)
            .HasConversion<int>()
            .IsRequired();

        builder.HasKey("RowId").IsClustered();

        builder.HasIndex("CountryId", "ContestId").IsUnique();
    }
}
