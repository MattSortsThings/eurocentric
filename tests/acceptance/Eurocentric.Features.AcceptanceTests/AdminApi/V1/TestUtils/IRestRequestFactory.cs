using Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

public interface IRestRequestFactory
{
    IContestsEndpoints Contests { get; }

    ICountriesEndpoints Countries { get; }

    interface IContestsEndpoints
    {
        RestRequest GetContest(Guid contestId);

        RestRequest GetContests();
    }

    interface ICountriesEndpoints
    {
        RestRequest CreateCountry(CreateCountryRequest requestBody);

        RestRequest GetCountries();

        RestRequest GetCountry(Guid countryId);
    }
}
