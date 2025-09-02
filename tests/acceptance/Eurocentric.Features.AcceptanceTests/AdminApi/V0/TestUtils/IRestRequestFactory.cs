using Eurocentric.Features.AdminApi.V0.Countries.CreateCountry;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils;

public interface IRestRequestFactory
{
    ICountriesEndpoints Countries { get; }

    interface ICountriesEndpoints
    {
        RestRequest CreateCountry(CreateCountryRequest requestBody);

        RestRequest GetCountries();

        RestRequest GetCountry(Guid countryId);
    }
}
