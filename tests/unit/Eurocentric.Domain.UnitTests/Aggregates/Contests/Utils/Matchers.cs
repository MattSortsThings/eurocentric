using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.UnitTests.Aggregates.Contests.Utils;

public static class Matchers
{
    public static Func<Participant, bool> Group0Participant(CountryId countryId)
    {
        CountryId expectedCountryId = countryId;

        return p => p.ParticipantGroup == ParticipantGroup.Zero
                    && p.ParticipatingCountryId.Equals(expectedCountryId)
                    && p.ActName is null
                    && p.SongTitle is null;
    }

    public static Func<Participant, bool> Group1Participant(CountryId countryId, string songTitle = "", string actName = "")
    {
        CountryId expectedCountryId = countryId;
        string expectedSongTitleValue = songTitle;
        string expectedActNameValue = actName;

        return p => p.ParticipantGroup == ParticipantGroup.One
                    && p.ParticipatingCountryId.Equals(expectedCountryId)
                    && p.ActName is { } a && a.Value.Equals(expectedActNameValue)
                    && p.SongTitle is { } s && s.Value.Equals(expectedSongTitleValue);
    }

    public static Func<Participant, bool> Group2Participant(CountryId countryId, string songTitle = "", string actName = "")
    {
        CountryId expectedCountryId = countryId;
        string expectedSongTitleValue = songTitle;
        string expectedActNameValue = actName;

        return p => p.ParticipantGroup == ParticipantGroup.Two
                    && p.ParticipatingCountryId.Equals(expectedCountryId)
                    && p.ActName is { } a && a.Value.Equals(expectedActNameValue)
                    && p.SongTitle is { } s && s.Value.Equals(expectedSongTitleValue);
    }
}
