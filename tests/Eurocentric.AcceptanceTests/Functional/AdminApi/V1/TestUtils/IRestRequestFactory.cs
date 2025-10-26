using Eurocentric.Apis.Admin.V1.Features.Contests;
using Eurocentric.Apis.Admin.V1.Features.Countries;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public interface IRestRequestFactory
{
    IContestsEndpoints Contests { get; }

    ICountriesEndpoints Countries { get; }

    interface IContestsEndpoints
    {
        RestRequest CreateContest(CreateContestRequest request);

        RestRequest GetContest(Guid contestId);

        RestRequest GetContests();
    }

    interface ICountriesEndpoints
    {
        RestRequest CreateCountry(CreateCountryRequest request);

        RestRequest GetCountries();

        RestRequest GetCountry(Guid countryId);
    }
}
