using Eurocentric.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.AdminApi.V1.Models;

namespace Eurocentric.AdminApi.Tests.Utils.V1.SampleData;

public static class TestCommands
{
    public static CreateCountryCommand CreateRealCountry() => new()
    {
        CountryCode = "GB", CountryName = "United Kingdom", CountryType = CountryType.Real
    };

    public static CreateCountryCommand CreatePseudoCountry() => new()
    {
        CountryCode = "XX", CountryName = "Rest of the World", CountryType = CountryType.Pseudo
    };
}
