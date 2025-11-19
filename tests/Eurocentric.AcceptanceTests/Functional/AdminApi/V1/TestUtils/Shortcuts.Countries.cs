using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Features.Countries;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public static partial class Shortcuts
{
    public static async IAsyncEnumerable<Country> CreateMultipleCountriesAsync(
        this AdminKernel kernel,
        params string[] countryCodes
    )
    {
        foreach (string countryCode in countryCodes)
        {
            yield return await kernel.CreateACountryAsync(countryCode: countryCode, countryName: "CountryName");
        }
    }

    extension(AdminKernel kernel)
    {
        public async Task<Country> CreateACountryAsync(string countryName = "", string countryCode = "")
        {
            RestRequest request = kernel.Requests.Countries.CreateCountry(
                new CreateCountryRequest { CountryCode = countryCode, CountryName = countryName }
            );

            ProblemOrResponse<CreateCountryResponse> response = await kernel.Client.SendAsync<CreateCountryResponse>(
                request
            );

            return response.AsResponse.Data!.Country;
        }

        public async Task DeleteACountryAsync(Guid countryId)
        {
            RestRequest request = kernel.Requests.Countries.DeleteCountry(countryId);
            _ = await kernel.Client.SendAsync(request);
        }

        public async Task<Country> GetACountryAsync(Guid countryId)
        {
            RestRequest request = kernel.Requests.Countries.GetCountry(countryId);
            ProblemOrResponse<GetCountryResponse> response = await kernel.Client.SendAsync<GetCountryResponse>(request);

            return response.AsResponse.Data!.Country;
        }

        public async Task<Country[]> GetAllCountriesAsync()
        {
            RestRequest request = kernel.Requests.Countries.GetCountries();

            ProblemOrResponse<GetCountriesResponse> response = await kernel.Client.SendAsync<GetCountriesResponse>(
                request
            );

            return response.AsResponse.Data!.Countries;
        }
    }
}
