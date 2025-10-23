using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V0.Dtos.Countries;
using Eurocentric.Apis.Admin.V0.Enums;
using Eurocentric.Apis.Admin.V0.Features.Countries;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V0.TestUtils;

public static class CountryShortcuts
{
    public static async Task<Country> CreateACountryAsync(
        this AdminKernel kernel,
        string countryName = "CountryName",
        string countryCode = "",
        CancellationToken cancellationToken = default
    )
    {
        CreateCountryRequest requestBody = new()
        {
            CountryType = CountryType.Real,
            CountryCode = countryCode,
            CountryName = countryName,
        };

        RestRequest request = kernel.Requests.Countries.CreateCountry(requestBody);

        ProblemOrResponse<CreateCountryResponse> problemOrResponse =
            await kernel.Client.SendAsync<CreateCountryResponse>(request, cancellationToken);

        return problemOrResponse.AsResponse.Data!.Country;
    }

    public static async Task<Country> GetACountryAsync(
        this AdminKernel kernel,
        Guid countryId,
        CancellationToken cancellationToken = default
    )
    {
        RestRequest request = kernel.Requests.Countries.GetCountry(countryId);

        ProblemOrResponse<GetCountryResponse> problemOrResponse = await kernel.Client.SendAsync<GetCountryResponse>(
            request,
            cancellationToken
        );

        return problemOrResponse.AsResponse.Data!.Country;
    }

    public static async Task<Country[]> GetAllCountriesAsync(
        this AdminKernel kernel,
        CancellationToken cancellationToken = default
    )
    {
        RestRequest request = kernel.Requests.Countries.GetCountries();

        ProblemOrResponse<GetCountriesResponse> problemOrResponse = await kernel.Client.SendAsync<GetCountriesResponse>(
            request,
            cancellationToken
        );

        return problemOrResponse.AsResponse.Data!.Countries;
    }

    public static async Task DeleteACountryAsync(
        this AdminKernel kernel,
        Guid countryId,
        CancellationToken cancellationToken = default
    )
    {
        RestRequest request = kernel.Requests.Countries.DeleteCountry(countryId);

        ProblemOrResponse problemOrResponse = await kernel.Client.SendAsync(request, cancellationToken);

        await Assert.That(problemOrResponse.AsResponse).IsNotNull();
    }
}
