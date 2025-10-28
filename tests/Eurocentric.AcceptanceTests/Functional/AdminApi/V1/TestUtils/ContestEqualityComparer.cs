using Eurocentric.Apis.Admin.V1.Dtos.Contests;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public sealed class ContestEqualityComparer : IEqualityComparer<Contest>
{
    public bool Equals(Contest? x, Contest? y)
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
            && x.ContestYear == y.ContestYear
            && x.CityName.Equals(y.CityName, StringComparison.Ordinal)
            && x.ContestRules == y.ContestRules
            && x.Queryable == y.Queryable
            && x.ChildBroadcasts.OrderBy(broadcast => broadcast.ChildBroadcastId)
                .SequenceEqual(
                    y.ChildBroadcasts.OrderBy(broadcast => broadcast.ChildBroadcastId),
                    new ChildBroadcastEqualityComparer()
                )
            && Equals(x.GlobalTelevote, y.GlobalTelevote)
            && x.Participants.OrderBy(participant => participant.ParticipatingCountryId)
                .SequenceEqual(
                    y.Participants.OrderBy(participant => participant.ParticipatingCountryId),
                    new ParticipantEqualityComparer()
                );
    }

    public int GetHashCode(Contest obj)
    {
        return HashCode.Combine(
            obj.Id,
            obj.ContestYear,
            obj.CityName,
            (int)obj.ContestRules,
            obj.Queryable,
            obj.ChildBroadcasts,
            obj.GlobalTelevote,
            obj.Participants
        );
    }
}
