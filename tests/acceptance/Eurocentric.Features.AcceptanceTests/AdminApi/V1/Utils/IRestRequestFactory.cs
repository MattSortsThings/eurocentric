using Eurocentric.Features.AdminApi.V1.Contests.CreateContest;
using Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public interface IRestRequestFactory
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
