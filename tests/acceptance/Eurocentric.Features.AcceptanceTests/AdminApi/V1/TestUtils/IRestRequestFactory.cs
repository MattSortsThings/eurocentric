using Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

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
