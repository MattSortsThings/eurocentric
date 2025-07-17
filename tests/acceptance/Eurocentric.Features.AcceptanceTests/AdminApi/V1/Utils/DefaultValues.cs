using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public static class DefaultValues
{
    public const int ContestYear = 2016;
    public const string CountryCode = "AA";
    public const string CityName = "CityName";
    public const string CountryName = "CountryName";
    public const string ActName = "ActName";
    public const string SongTitle = "SongTitle";

    public static ContestParticipantSpecification ParticipantSpec(Guid participatingCountryId) =>
        new(participatingCountryId, ActName, SongTitle);
}
