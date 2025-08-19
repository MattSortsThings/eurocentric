using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;

public interface IRestRequestFactory
{
    IQueryablesEndpoints Queryables { get; }

    IRankingsEndpoints Rankings { get; }

    interface IQueryablesEndpoints
    {
        RestRequest GetQueryableBroadcasts();

        RestRequest GetQueryableContestStages();

        RestRequest GetQueryableContests();

        RestRequest GetQueryableCountries();

        RestRequest GetQueryableVotingMethods();
    }

    interface IRankingsEndpoints
    {
        RestRequest GetCompetingCountryPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetCompetingCountryPointsConsensusRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetCompetingCountryPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetCompetingCountryPointsShareRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetCompetitorPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetCompetitorPointsConsensusRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetCompetitorPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetCompetitorPointsShareRankings(IReadOnlyDictionary<string, object?> queryParams);
    }
}
