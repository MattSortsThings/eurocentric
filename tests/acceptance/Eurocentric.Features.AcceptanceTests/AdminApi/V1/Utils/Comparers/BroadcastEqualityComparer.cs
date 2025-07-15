using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;

internal sealed class BroadcastEqualityComparer : IEqualityComparer<Broadcast>
{
    public bool Equals(Broadcast? x, Broadcast? y)
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

        return x.Id.Equals(y.Id)
               && x.BroadcastDate.Equals(y.BroadcastDate)
               && x.ParentContestId.Equals(y.ParentContestId)
               && x.ContestStage == y.ContestStage
               && x.Completed == y.Completed
               && x.Competitors.SequenceEqual(y.Competitors, new CompetitorEqualityComparer())
               && x.Juries.SequenceEqual(y.Juries)
               && x.Televotes.SequenceEqual(y.Televotes);
    }

    public int GetHashCode(Broadcast obj) => HashCode.Combine(obj.Id,
        obj.BroadcastDate,
        obj.ParentContestId,
        (int)obj.ContestStage,
        obj.Completed,
        obj.Competitors,
        obj.Juries,
        obj.Televotes);

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
