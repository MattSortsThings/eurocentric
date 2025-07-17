using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public sealed class BroadcastEqualityComparer : IEqualityComparer<Broadcast>
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
               && x.Competitors.OrderBy(competitor => competitor.FinishingPosition)
                   .SequenceEqual(y.Competitors.OrderBy(competitor => competitor.FinishingPosition),
                       new CompetitorEqualityComparer())
               && x.Juries.OrderBy(voter => voter.PointsAwarded)
                   .ThenBy(voter => voter.VotingCountryId)
                   .SequenceEqual(y.Juries.OrderBy(voter => voter.PointsAwarded)
                       .ThenBy(voter => voter.VotingCountryId))
               && x.Televotes.OrderBy(voter => voter.PointsAwarded)
                   .ThenBy(voter => voter.VotingCountryId)
                   .SequenceEqual(y.Televotes.OrderBy(voter => voter.PointsAwarded)
                       .ThenBy(voter => voter.VotingCountryId));
    }

    public int GetHashCode(Broadcast obj) => HashCode.Combine(obj.Id,
        obj.BroadcastDate,
        obj.ParentContestId,
        (int)obj.ContestStage,
        obj.Completed,
        obj.Competitors,
        obj.Juries,
        obj.Televotes);
}
