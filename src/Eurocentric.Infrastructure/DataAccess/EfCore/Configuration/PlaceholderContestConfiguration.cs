using Eurocentric.Domain.Enums;
using Eurocentric.Domain.PlaceholderEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Configuration;

internal sealed class PlaceholderContestConfiguration : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.ToTable("placeholder_contest");

        builder.Property(contest => contest.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(contest => contest.ContestYear)
            .IsRequired();

        builder.Property(contest => contest.CityName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(contest => contest.ContestFormat)
            .HasConversion<int>()
            .IsRequired();

        builder.HasKey(contest => contest.Id);

        builder.HasAlternateKey(contest => contest.ContestYear);

        builder.ToTable(AddContestFormatEnumCheckConstraint);
    }

    private static void AddContestFormatEnumCheckConstraint(TableBuilder<Contest> builder) =>
        builder.HasCheckConstraint("ck_placeholder_contest_contest_format_enum",
            SqlHelpers.CreateEnumIntValueCheckConstraint<ContestFormat>("contest_format"));
}
