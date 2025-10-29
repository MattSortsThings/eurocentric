using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Contests;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public static class TestDefaults
{
    public const int ContestYear = 2016;
    public const string CityName = "CityName";
    public const string ActName = "ActName";
    public const string SongTitle = "SongTitle";

    public static CreateParticipantRequest SemiFinal1ParticipantRequest(Guid countryId) =>
        new()
        {
            ParticipatingCountryId = countryId,
            SemiFinalDraw = SemiFinalDraw.SemiFinal1,
            ActName = ActName,
            SongTitle = SongTitle,
        };

    public static CreateParticipantRequest SemiFinal2ParticipantRequest(Guid countryId) =>
        new()
        {
            ParticipatingCountryId = countryId,
            SemiFinalDraw = SemiFinalDraw.SemiFinal2,
            ActName = ActName,
            SongTitle = SongTitle,
        };
}
