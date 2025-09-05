using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.Features.AdminApi.V1.Countries.GetCountries;
using Eurocentric.Features.AdminApi.V1.Countries.GetCountry;
using RestSharp;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Country;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.Helpers.Countries;

public static class ApiDriverExtensions
{
    public static async IAsyncEnumerable<CountryDto> CreateMultipleCountriesAsync(
        this IApiDriver apiDriver,
        params string[] countryCodes)
    {
        foreach (CreateCountryRequest requestBody in countryCodes.Select(MapToRequestBody))
        {
            RestRequest request = apiDriver.RequestFactory.Countries.CreateCountry(requestBody);

            BiRestResponse<CreateCountryResponse> response = await apiDriver.RestClient.SendAsync<CreateCountryResponse>(request,
                TestContext.Current!.CancellationToken);

            CreateCountryResponse responseBody = await Assert.That(response.AsSuccessful.Data).IsNotNull();

            yield return responseBody.Country;
        }
    }

    public static async Task<CountryDto> CreateSingleCountryAsync(
        this IApiDriver apiDriver,
        string countryName = TestDefaults.CountryName,
        string countryCode = "")
    {
        CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = countryName };

        RestRequest request = apiDriver.RequestFactory.Countries.CreateCountry(requestBody);

        BiRestResponse<CreateCountryResponse> response = await apiDriver.RestClient.SendAsync<CreateCountryResponse>(request,
            TestContext.Current!.CancellationToken);

        CreateCountryResponse responseBody = await Assert.That(response.AsSuccessful.Data).IsNotNull();

        return responseBody.Country;
    }

    public static async Task DeleteSingleCountryAsync(this IApiDriver apiDriver, Guid countryId)
    {
        CountryId countryIdToDelete = CountryId.FromValue(countryId);

        await apiDriver.BackDoor.ExecuteScopedAsync(BackDoorOperations.DeleteCountryAsync(countryIdToDelete));
    }

    public static async Task<CountryDto> GetSingleCountryAsync(this IApiDriver apiDriver, Guid countryId)
    {
        RestRequest request = apiDriver.RequestFactory.Countries.GetCountry(countryId);

        BiRestResponse<GetCountryResponse> response = await apiDriver.RestClient.SendAsync<GetCountryResponse>(request,
            TestContext.Current!.CancellationToken);

        GetCountryResponse responseBody = await Assert.That(response.AsSuccessful.Data).IsNotNull();

        return responseBody.Country;
    }

    public static async Task<CountryDto[]> GetAllCountriesAsync(this IApiDriver apiDriver)
    {
        RestRequest request = apiDriver.RequestFactory.Countries.GetCountries();

        BiRestResponse<GetCountriesResponse> response = await apiDriver.RestClient.SendAsync<GetCountriesResponse>(request,
            TestContext.Current!.CancellationToken);

        GetCountriesResponse responseBody = await Assert.That(response.AsSuccessful.Data).IsNotNull();

        return responseBody.Countries;
    }

    private static CreateCountryRequest MapToRequestBody(string countryCode) => new()
    {
        CountryCode = countryCode, CountryName = TestDefaults.CountryName
    };
}
