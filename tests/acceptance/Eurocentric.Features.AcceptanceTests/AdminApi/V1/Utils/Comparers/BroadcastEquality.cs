using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;

internal static class BroadcastEquality
{
    internal static bool Compare(Broadcast a, Broadcast b) =>
        a.Id == b.Id
        && a.ParentContestId == b.ParentContestId
        && a.ContestStage == b.ContestStage
        && a.BroadcastDate == b.BroadcastDate
        && a.Completed == b.Completed
        && a.Competitors.SequenceEqual(b.Competitors, new CompetitorEqualityComparer())
        && a.Juries.SequenceEqual(b.Juries)
        && a.Televotes.SequenceEqual(b.Televotes);

    private sealed class CompetitorEqualityComparer : IEqualityComparer<Competitor>
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
}
