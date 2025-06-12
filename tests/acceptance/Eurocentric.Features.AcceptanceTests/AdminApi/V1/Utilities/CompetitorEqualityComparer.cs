using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

internal sealed class CompetitorEqualityComparer : IEqualityComparer<Competitor>
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
               && x.FinishingPosition == y.FinishingPosition
               && x.RunningOrderPosition == y.RunningOrderPosition
               && x.JuryAwards.SequenceEqual(y.JuryAwards)
               && x.TelevoteAwards.SequenceEqual(y.TelevoteAwards);
    }

    public int GetHashCode(Competitor obj) => HashCode.Combine(obj.CompetingCountryId,
        obj.FinishingPosition,
        obj.RunningOrderPosition,
        obj.JuryAwards,
        obj.TelevoteAwards);
}
