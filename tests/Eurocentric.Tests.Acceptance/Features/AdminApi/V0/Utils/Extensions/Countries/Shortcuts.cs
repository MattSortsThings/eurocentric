using Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;
using Eurocentric.Apis.Admin.V0.Common.Enums;
using Eurocentric.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Utils.Extensions.Countries;

public static class Shortcuts
{
    extension(IActorKernel kernel)
    {
        public async Task<Country> CreateSingleCountryAsync(string countryName = "", string countryCode = "")
        {
            RestRequest request = kernel.RestRequestFactory.CreateCountry(
                new CreateCountryRequest
                {
                    CountryType = CountryType.Real,
                    CountryCode = countryCode,
                    CountryName = countryName,
                }
            );

            CreateCountryResponse responseBody = await kernel.RestClient.SendSafeRequestAsync<CreateCountryResponse>(
                request
            );

            return responseBody.Country;
        }

        public async Task DeleteSingleCountryAsync(Guid countryId)
        {
            RestRequest request = kernel.RestRequestFactory.DeleteCountry(countryId);

            await kernel.RestClient.SendSafeRequestAsync(request);
        }

        public async Task<Country[]> GetAllCountriesAsync()
        {
            RestRequest request = kernel.RestRequestFactory.GetCountries();

            GetCountriesResponse responseBody = await kernel.RestClient.SendSafeRequestAsync<GetCountriesResponse>(
                request
            );

            return responseBody.Countries;
        }
    }
}
