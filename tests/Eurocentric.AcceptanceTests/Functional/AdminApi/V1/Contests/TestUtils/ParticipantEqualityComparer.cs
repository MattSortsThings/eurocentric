using Eurocentric.Apis.Admin.V1.Dtos.Contests;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests.TestUtils;

public sealed class ParticipantEqualityComparer : IEqualityComparer<Participant>
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
            && x.SemiFinalDraw == y.SemiFinalDraw
            && x.ActName.Equals(y.ActName, StringComparison.Ordinal)
            && x.SongTitle.Equals(y.SongTitle, StringComparison.Ordinal);
    }

    public int GetHashCode(Participant obj) =>
        HashCode.Combine(obj.ParticipatingCountryId, (int)obj.SemiFinalDraw, obj.ActName, obj.SongTitle);
}
