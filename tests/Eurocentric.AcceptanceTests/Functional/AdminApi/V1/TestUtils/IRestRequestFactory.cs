using Eurocentric.Apis.Admin.V1.Features.Countries;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public interface IRestRequestFactory
{
    ICountriesEndpoints Countries { get; }

    interface ICountriesEndpoints
    {
        RestRequest CreateCountry(CreateCountryRequest request);

        RestRequest GetCountries();

        RestRequest GetCountry(Guid countryId);
    }
}
