using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;

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
               && x.RunningOrderPosition == y.RunningOrderPosition
               && x.FinishingPosition == y.FinishingPosition
               && x.JuryAwards.OrderBy(award => award.VotingCountryId)
                   .SequenceEqual(y.JuryAwards.OrderBy(award => award.VotingCountryId))
               && x.TelevoteAwards.OrderBy(award => award.VotingCountryId)
                   .SequenceEqual(y.TelevoteAwards.OrderBy(award => award.VotingCountryId));
    }

    public int GetHashCode(Competitor obj) => HashCode.Combine(obj.CompetingCountryId,
        obj.RunningOrderPosition,
        obj.FinishingPosition,
        obj.JuryAwards,
        obj.TelevoteAwards);
}
