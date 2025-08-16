using Eurocentric.Features.AdminApi.V1.Broadcasts.AwardJuryPoints;
using Eurocentric.Features.AdminApi.V1.Broadcasts.AwardTelevotePoints;
using Eurocentric.Features.AdminApi.V1.Broadcasts.DisqualifyCompetitor;
using Eurocentric.Features.AdminApi.V1.Contests.CreateChildBroadcast;
using Eurocentric.Features.AdminApi.V1.Contests.CreateContest;
using Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public interface IRestRequestFactory
{
    IBroadcastsEndpoints Broadcasts { get; }

    IContestsEndpoints Contests { get; }

    ICountriesEndpoints Countries { get; }

    interface IBroadcastsEndpoints
    {
        RestRequest AwardJuryPoints(Guid broadcastId, AwardJuryPointsRequest requestBody);

        RestRequest AwardTelevotePoints(Guid broadcastId, AwardTelevotePointsRequest requestBody);

        RestRequest DisqualifyCompetitor(Guid broadcastId, DisqualifyCompetitorRequest requestBody);

        RestRequest DeleteBroadcast(Guid broadcastId);

        RestRequest GetBroadcast(Guid broadcastId);

        RestRequest GetBroadcasts();
    }

    interface IContestsEndpoints
    {
        RestRequest CreateContest(CreateContestRequest requestBody);

        RestRequest CreateChildBroadcast(Guid contestId, CreateChildBroadcastRequest requestBody);

        RestRequest DeleteContest(Guid contestId);

        RestRequest GetContest(Guid contestId);

        RestRequest GetContests();
    }

    interface ICountriesEndpoints
    {
        RestRequest CreateCountry(CreateCountryRequest requestBody);

        RestRequest DeleteCountry(Guid countryId);

        RestRequest GetCountries();

        RestRequest GetCountry(Guid countryId);
    }
}
