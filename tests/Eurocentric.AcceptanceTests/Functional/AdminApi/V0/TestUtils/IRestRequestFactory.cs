using Eurocentric.Apis.Admin.V0.Features.Countries;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V0.TestUtils;

public interface IRestRequestFactory
{
    ICountriesEndpoints Countries { get; }

    interface ICountriesEndpoints
    {
        RestRequest CreateCountry(CreateCountryRequest requestBody);
        RestRequest DeleteCountry(Guid countryId);
        RestRequest GetCountries();
        RestRequest GetCountry(Guid countryId);
    }
}
