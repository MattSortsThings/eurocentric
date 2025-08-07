using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;

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
               && x.Competitors.OrderBy(competitor => competitor.CompetingCountryId)
                   .SequenceEqual(y.Competitors.OrderBy(competitor => competitor.CompetingCountryId),
                       new CompetitorEqualityComparer())
               && x.Juries.OrderBy(voter => voter.VotingCountryId)
                   .SequenceEqual(y.Juries.OrderBy(voter => voter.VotingCountryId))
               && x.Televotes.OrderBy(voter => voter.VotingCountryId)
                   .SequenceEqual(y.Televotes.OrderBy(voter => voter.VotingCountryId));
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
