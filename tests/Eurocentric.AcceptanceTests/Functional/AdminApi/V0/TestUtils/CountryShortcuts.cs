using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V0.Dtos.Countries;
using Eurocentric.Apis.Admin.V0.Enums;
using Eurocentric.Apis.Admin.V0.Features.Countries;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V0.TestUtils;

public static class CountryShortcuts
{
    extension(AdminKernel kernel)
    {
        public async Task<Country> CreateACountryAsync(string countryName = "CountryName", string countryCode = "")
        {
            CreateCountryRequest requestBody = new()
            {
                CountryType = CountryType.Real,
                CountryCode = countryCode,
                CountryName = countryName,
            };

            RestRequest request = kernel.Requests.Countries.CreateCountry(requestBody);

            ProblemOrResponse<CreateCountryResponse> problemOrResponse =
                await kernel.Client.SendAsync<CreateCountryResponse>(request);

            return problemOrResponse.AsResponse.Data!.Country;
        }

        public async Task<Country> GetACountryAsync(Guid countryId)
        {
            RestRequest request = kernel.Requests.Countries.GetCountry(countryId);

            ProblemOrResponse<GetCountryResponse> problemOrResponse = await kernel.Client.SendAsync<GetCountryResponse>(
                request
            );

            return problemOrResponse.AsResponse.Data!.Country;
        }

        public async Task<Country[]> GetAllCountriesAsync()
        {
            RestRequest request = kernel.Requests.Countries.GetCountries();

            ProblemOrResponse<GetCountriesResponse> problemOrResponse =
                await kernel.Client.SendAsync<GetCountriesResponse>(request);

            return problemOrResponse.AsResponse.Data!.Countries;
        }

        public async Task DeleteACountryAsync(Guid countryId)
        {
            RestRequest request = kernel.Requests.Countries.DeleteCountry(countryId);

            ProblemOrResponse problemOrResponse = await kernel.Client.SendAsync(request);

            await Assert.That(problemOrResponse.AsResponse).IsNotNull();
        }
    }
}
