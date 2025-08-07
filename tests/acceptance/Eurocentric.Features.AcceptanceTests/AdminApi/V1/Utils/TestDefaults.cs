using Eurocentric.Features.AdminApi.V1.Contests.CreateContest;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public static class TestDefaults
{
    public const int ContestYear = 2025;
    public const string CountryName = "CountryName";
    public const string CountryCode = "AA";
    public const string CityName = "CityName";
    public const string DateFormat = "yyyy-MM-dd";

    public static ContestParticipantDatum ParticipantDatum(Guid countryId) => new()
    {
        ParticipatingCountryId = countryId, ActName = "ActName", SongTitle = "SongTitle"
    };
}
