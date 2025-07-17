using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

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
               && x.Completed == y.Completed
               && x.ContestFormat == y.ContestFormat
               && x.ChildBroadcasts.OrderBy(memo => memo.ContestStage)
                   .SequenceEqual(y.ChildBroadcasts.OrderBy(memo => memo.ContestStage))
               && x.Participants.OrderBy(participant => participant.ParticipantGroup)
                   .ThenBy(participant => participant.ParticipatingCountryId)
                   .SequenceEqual(y.Participants.OrderBy(participant => participant.ParticipantGroup)
                       .ThenBy(participant => participant.ParticipatingCountryId));
    }

    public int GetHashCode(Contest obj) => HashCode.Combine(obj.Id,
        obj.ContestYear,
        obj.CityName,
        obj.Completed,
        (int)obj.ContestFormat, obj.ChildBroadcasts, obj.Participants);
}
