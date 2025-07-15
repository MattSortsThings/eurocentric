using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Features.AdminApi.V1.Countries;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public interface IAdminApiV1RequestFactory
{
    public IContestsEndpoints Contests { get; }

    public ICountriesEndpoints Countries { get; }

    public interface IContestsEndpoints
    {
        public RestRequest CreateContest(CreateContestRequest requestBody);

        public RestRequest GetContest(Guid contestId);

        public RestRequest GetContests();
    }

    public interface ICountriesEndpoints
    {
        public RestRequest CreateCountry(CreateCountryRequest requestBody);

        public RestRequest GetCountries();

        public RestRequest GetCountry(Guid countryId);
    }
}
