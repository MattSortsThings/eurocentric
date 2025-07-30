using Eurocentric.Domain.V0Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Configuration;

internal sealed class V0CountryConfig : IEntityTypeConfiguration<Country>
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

        builder.OwnsMany(country => country.ParticipatingContestIds, ConfigureAsTable);

        builder.HasKey(country => country.Id).IsClustered();

        builder.HasAlternateKey(country => country.CountryCode);
    }

    private static void ConfigureAsTable(OwnedNavigationBuilder<Country, ContestMemo> builder)
    {
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.ToTable("country_participating_contest", "v0");

        builder.Property<int>("Id")
            .IsRequired()
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.WithOwner()
            .HasForeignKey("CountryId")
            .HasPrincipalKey(country => country.Id);

        builder.Property(memo => memo.ContestId)
            .HasColumnName("participating_contest_id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.HasKey("Id").IsClustered();
    }
}
