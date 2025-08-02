using Eurocentric.Features.AdminApi.V0.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Comparers;

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
               && x.CityName == y.CityName
               && x.ContestFormat == y.ContestFormat
               && x.Completed == y.Completed
               && x.ChildBroadcasts.OrderBy(broadcast => broadcast.BroadcastId)
                   .SequenceEqual(y.ChildBroadcasts.OrderBy(broadcast => broadcast.BroadcastId))
               && x.Participants.OrderBy(participant => participant.ParticipatingCountryId)
                   .SequenceEqual(y.Participants.OrderBy(participant => participant.ParticipatingCountryId));
    }

    public int GetHashCode(Contest obj) => HashCode.Combine(obj.Id, obj.ContestYear, obj.CityName, (int)obj.ContestFormat,
        obj.Completed, obj.ChildBroadcasts, obj.Participants);
}
