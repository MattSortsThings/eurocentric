using Eurocentric.Apis.Admin.V0.Features.Countries;
using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.AdminApi.V0.Utils;

public interface IRestRequestFactory
{
    ICountriesEndpoints Countries { get; }

    interface ICountriesEndpoints
    {
        RestRequest CreateCountry(CreateCountry.Request requestBody);

        RestRequest GetCountry(Guid countryId);

        RestRequest GetCountries();
    }
}
