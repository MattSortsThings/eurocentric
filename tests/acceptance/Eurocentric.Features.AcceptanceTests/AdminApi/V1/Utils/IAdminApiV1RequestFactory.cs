using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public interface IAdminApiV1RequestFactory
{
    public ICountriesEndpoints Countries { get; }

    public interface ICountriesEndpoints
    {
        public RestRequest GetCountries();

        public RestRequest GetCountry(Guid countryId);
    }
}
