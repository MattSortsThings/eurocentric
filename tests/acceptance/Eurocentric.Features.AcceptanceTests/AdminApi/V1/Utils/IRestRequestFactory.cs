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
    public IBroadcastsEndpoints Broadcasts { get; }

    public IContestsEndpoints Contests { get; }

    public ICountriesEndpoints Countries { get; }

    public interface IBroadcastsEndpoints
    {
        public RestRequest AwardJuryPoints(Guid broadcastId, AwardJuryPointsRequest requestBody);

        public RestRequest AwardTelevotePoints(Guid broadcastId, AwardTelevotePointsRequest requestBody);

        public RestRequest DisqualifyCompetitor(Guid broadcastId, DisqualifyCompetitorRequest requestBody);

        public RestRequest DeleteBroadcast(Guid broadcastId);

        public RestRequest GetBroadcast(Guid broadcastId);

        public RestRequest GetBroadcasts();
    }

    public interface IContestsEndpoints
    {
        public RestRequest CreateContest(CreateContestRequest requestBody);

        public RestRequest CreateChildBroadcast(Guid contestId, CreateChildBroadcastRequest requestBody);

        public RestRequest DeleteContest(Guid contestId);

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
