using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Broadcasts.TestUtils;

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
            && x.Competitors.OrderBy(competitor => competitor.CompetingCountryId)
                .SequenceEqual(y.Competitors.OrderBy(competitor => competitor.CompetingCountryId))
            && x.Juries.OrderBy(jury => jury.VotingCountryId)
                .SequenceEqual(y.Juries.OrderBy(jury => jury.VotingCountryId))
            && x.Televotes.OrderBy(televote => televote.VotingCountryId)
                .SequenceEqual(y.Televotes.OrderBy(televote => televote.VotingCountryId));
    }

    public int GetHashCode(Broadcast obj) =>
        HashCode.Combine(
            obj.Id,
            obj.BroadcastDate,
            obj.ParentContestId,
            (int)obj.ContestStage,
            obj.Competitors,
            obj.Juries,
            obj.Televotes
        );
}
