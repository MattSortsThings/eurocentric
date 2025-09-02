using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Countries.CreateCountry;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils.Helpers.Countries;

public static class ApiDriverExtensions
{
    internal static async Task<Country> CreateSingleCountryAsync(
        this IApiDriver apiDriver,
        string countryCode = "",
        string countryName = "")
    {
        CreateCountryRequest requestBody = new()
        {
            CountryCode = countryCode, CountryName = countryName, CountryType = CountryType.Real
        };

        RestRequest request = apiDriver.RequestFactory.Countries.CreateCountry(requestBody);
        BiRestResponse<CreateCountryResponse> response = await apiDriver.RestClient.SendAsync<CreateCountryResponse>(request);

        return response.AsSuccessful.Data!.Country;
    }
}
