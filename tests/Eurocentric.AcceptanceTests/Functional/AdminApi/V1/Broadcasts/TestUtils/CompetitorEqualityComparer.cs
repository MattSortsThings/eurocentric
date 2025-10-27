using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Broadcasts.TestUtils;

public sealed class CompetitorEqualityComparer : IEqualityComparer<Competitor>
{
    public bool Equals(Competitor? x, Competitor? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null)
        {
            return false;
        }

        if (y is null)
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return x.CompetingCountryId.Equals(y.CompetingCountryId)
            && x.RunningOrderSpot == y.RunningOrderSpot
            && x.FinishingPosition == y.FinishingPosition
            && x.JuryAwards.OrderBy(award => award.VotingCountryId)
                .SequenceEqual(y.JuryAwards.OrderBy(award => award.VotingCountryId))
            && x.TelevoteAwards.OrderBy(award => award.VotingCountryId)
                .SequenceEqual(y.TelevoteAwards.OrderBy(award => award.VotingCountryId));
    }

    public int GetHashCode(Competitor obj) =>
        HashCode.Combine(
            obj.CompetingCountryId,
            obj.RunningOrderSpot,
            obj.FinishingPosition,
            obj.JuryAwards,
            obj.TelevoteAwards
        );
}
