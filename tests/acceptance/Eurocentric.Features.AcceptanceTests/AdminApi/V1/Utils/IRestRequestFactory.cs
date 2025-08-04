using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public interface IRestRequestFactory
{
    public ICountriesEndpoints Countries { get; }

    public interface ICountriesEndpoints
    {
        public RestRequest GetCountries();

        public RestRequest GetCountry(Guid countryId);
    }
}
