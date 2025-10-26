using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.UnitTests.Domain.Aggregates.Contests.TestUtils;

public static class Matchers
{
    public static Func<Participant, bool> Participant(
        string songTitle = "",
        string actName = "",
        SemiFinalDraw semiFinalDraw = default,
        CountryId participatingCountryId = null!
    )
    {
        return participant =>
            participant.ParticipatingCountryId == participatingCountryId
            && participant.SemiFinalDraw == semiFinalDraw
            && participant.ActName.Value.Equals(actName, StringComparison.Ordinal)
            && participant.SongTitle.Value.Equals(songTitle, StringComparison.Ordinal);
    }
}
