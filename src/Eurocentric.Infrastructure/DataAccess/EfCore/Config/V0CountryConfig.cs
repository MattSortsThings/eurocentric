using Eurocentric.Domain.V0.Aggregates.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config;

internal sealed class V0CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("country", "v0");

        builder
            .Property(country => country.Id)
            .HasColumnName("country_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(country => country.CountryCode)
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired();

        builder.Property(country => country.CountryName).HasMaxLength(200).IsRequired();

        builder.OwnsMany(country => country.ContestRoles, ConfigureAsTable);

        builder.HasKey(country => country.Id).IsClustered();

        builder.HasAlternateKey(country => country.CountryCode).IsClustered(false);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Country, ContestRole> builder)
    {
        builder.ToTable("contest_role", "v0");

        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.WithOwner().HasForeignKey("CountryId").HasPrincipalKey(country => country.Id);

        builder.Property<int>("Id").IsRequired().UseIdentityColumn().ValueGeneratedOnAdd();

        builder.Property(role => role.ContestId).IsRequired().ValueGeneratedNever();

        builder
            .Property(role => role.ContestRoleType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasKey("Id").IsClustered();

        builder.HasIndex("CountryId", "ContestId").IsUnique();

        builder.ToTable(AddContestRoleEnumCheckConstraint);
    }

    private static void AddContestRoleEnumCheckConstraint(
        OwnedNavigationTableBuilder<Country, ContestRole> builder
    )
    {
        builder.HasCheckConstraint(
            "ck_contest_role_contest_role_type_enum",
            "[contest_role_type] IN (N'Participant', N'GlobalTelevote')"
        );
    }
}
