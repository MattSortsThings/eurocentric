using Eurocentric.Apis.Admin.V1.Features.Broadcasts;
using Eurocentric.Apis.Admin.V1.Features.Contests;
using Eurocentric.Apis.Admin.V1.Features.Countries;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public interface IRestRequestFactory
{
    IBroadcastsEndpoints Broadcasts { get; }

    IContestsEndpoints Contests { get; }

    ICountriesEndpoints Countries { get; }

    interface IBroadcastsEndpoints
    {
        RestRequest AwardBroadcastJuryPoints(Guid broadcastId, AwardBroadcastJuryPointsRequest requestBody);

        RestRequest AwardBroadcastTelevotePoints(Guid broadcastId, AwardBroadcastTelevotePointsRequest requestBody);

        RestRequest DeleteBroadcast(Guid broadcastId);

        RestRequest GetBroadcast(Guid broadcastId);

        RestRequest GetBroadcasts();
    }

    interface IContestsEndpoints
    {
        RestRequest CreateContest(CreateContestRequest requestBody);

        RestRequest CreateContestBroadcast(Guid contestId, CreateContestBroadcastRequest requestBody);

        RestRequest DeleteContest(Guid contestId);

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
