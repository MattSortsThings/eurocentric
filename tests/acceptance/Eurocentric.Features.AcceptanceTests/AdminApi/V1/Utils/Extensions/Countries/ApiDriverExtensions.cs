using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.Features.AdminApi.V1.Countries.GetCountries;
using Eurocentric.Features.AdminApi.V1.Countries.GetCountry;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;

public static class ApiDriverExtensions
{
    public static async Task<List<Country>> CreateMultipleCountriesAsync(this IApiDriver driver, params string[] countryCodes)
    {
        List<Country> countries = new(countryCodes.Length);

        foreach (string countryCode in countryCodes)
        {
            Country country = await driver.CreateSingleCountryAsync(countryCode: countryCode);
            countries.Add(country);
        }

        return countries;
    }

    public static async Task<Country> CreateSingleCountryAsync(this IApiDriver driver,
        string countryName = TestDefaults.CountryName,
        string countryCode = "")
    {
        CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = countryName };

        RestRequest request = driver.RequestFactory.Countries.CreateCountry(requestBody);
        ProblemOrResponse<CreateCountryResponse> response = await driver.RestClient.SendAsync<CreateCountryResponse>(request);

        return response.AsResponse.Data!.Country;
    }

    public static async Task<Country[]> GetAllCountriesAsync(this IApiDriver driver)
    {
        RestRequest request = driver.RequestFactory.Countries.GetCountries();
        ProblemOrResponse<GetCountriesResponse> response = await driver.RestClient.SendAsync<GetCountriesResponse>(request);

        return response.AsResponse.Data!.Countries;
    }

    public static async Task<Country> GetSingleCountryAsync(this IApiDriver driver, Guid countryId)
    {
        RestRequest request = driver.RequestFactory.Countries.GetCountry(countryId);
        ProblemOrResponse<GetCountryResponse> response = await driver.RestClient.SendAsync<GetCountryResponse>(request);

        return response.AsResponse.Data!.Country;
    }

    public static async Task DeleteSingleCountryAsync(this IApiDriver driver, Guid countryId)
    {
        RestRequest request = driver.RequestFactory.Countries.DeleteCountry(countryId);
        ProblemOrResponse response = await driver.RestClient.SendAsync(request);

        await Assert.That(response.IsT1).IsTrue();
    }
}
