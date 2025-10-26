using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Contests;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public static class TestDefaults
{
    public const int DefaultContestYear = 2016;
    public const string DefaultCityName = "CityName";
    public const string DefaultActName = "ActName";
    public const string DefaultSongTitle = "SongTitle";

    public static CreateParticipantRequest SemiFinal1ParticipantRequest(Guid countryId) =>
        new()
        {
            ParticipatingCountryId = countryId,
            SemiFinalDraw = SemiFinalDraw.SemiFinal1,
            ActName = DefaultActName,
            SongTitle = DefaultSongTitle,
        };

    public static CreateParticipantRequest SemiFinal2ParticipantRequest(Guid countryId) =>
        new()
        {
            ParticipatingCountryId = countryId,
            SemiFinalDraw = SemiFinalDraw.SemiFinal2,
            ActName = DefaultActName,
            SongTitle = DefaultSongTitle,
        };
}
