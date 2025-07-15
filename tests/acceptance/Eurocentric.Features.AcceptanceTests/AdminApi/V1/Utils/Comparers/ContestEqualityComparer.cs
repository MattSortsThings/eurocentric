using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;

internal sealed class ContestEqualityComparer : IEqualityComparer<Contest>
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
               && x.ContestFormat == y.ContestFormat &&
               x.ChildBroadcasts.SequenceEqual(y.ChildBroadcasts)
               && x.Participants.SequenceEqual(y.Participants, new ParticipantEqualityComparer());
    }

    public int GetHashCode(Contest obj) => HashCode.Combine(obj.Id,
        obj.ContestYear,
        obj.CityName,
        obj.Completed,
        (int)obj.ContestFormat,
        obj.ChildBroadcasts,
        obj.Participants);

    private sealed class ParticipantEqualityComparer : IEqualityComparer<Participant>
    {
        public bool Equals(Participant? x, Participant? y)
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

            return x.ParticipatingCountryId.Equals(y.ParticipatingCountryId)
                   && x.ParticipantGroup == y.ParticipantGroup
                   && x.ActName == y.ActName
                   && x.SongTitle == y.SongTitle;
        }

        public int GetHashCode(Participant obj) => HashCode.Combine(obj.ParticipatingCountryId,
            obj.ParticipantGroup,
            obj.ActName,
            obj.SongTitle);
    }
}
