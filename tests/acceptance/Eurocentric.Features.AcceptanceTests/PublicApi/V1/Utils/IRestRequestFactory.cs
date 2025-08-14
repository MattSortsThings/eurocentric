using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;

public interface IRestRequestFactory
{
    public IQueryablesEndpoints Queryables { get; }

    public IRankingsEndpoints Rankings { get; }

    public interface IQueryablesEndpoints
    {
        public RestRequest GetQueryableBroadcasts();

        public RestRequest GetQueryableContestStages();

        public RestRequest GetQueryableContests();

        public RestRequest GetQueryableCountries();

        public RestRequest GetQueryableVotingMethods();
    }

    public interface IRankingsEndpoints
    {
        public RestRequest GetCompetingCountryPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams);
    }
}
