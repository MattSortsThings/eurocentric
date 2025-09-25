using Eurocentric.Domain.V0.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config;

internal sealed class V0QueryableContestConfig : IEntityTypeConfiguration<QueryableContest>
{
    public void Configure(EntityTypeBuilder<QueryableContest> builder)
    {
        builder.ToView("vw_queryable_contest", "v0");

        builder.Property(contest => contest.ContestYear).IsRequired();

        builder.Property(contest => contest.CityName).HasMaxLength(200).IsRequired();

        builder.Property(contest => contest.Participants).IsRequired();

        builder.Property(contest => contest.UsesRestOfWorldTelevote).IsRequired();

        builder.HasNoKey();
    }
}
