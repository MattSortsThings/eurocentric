using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;

public interface IRestRequestFactory
{
    ICompetingCountryRankingsEndpoints CompetingCountryRankings { get; }

    ICompetitorRankingsEndpoints CompetitorRankings { get; }

    IQueryablesEndpoints Queryables { get; }

    IVotingCountryRankingsEndpoints VotingCountryRankings { get; }

    interface IQueryablesEndpoints
    {
        RestRequest GetQueryableBroadcasts();

        RestRequest GetQueryableContestStages();

        RestRequest GetQueryableContests();

        RestRequest GetQueryableCountries();

        RestRequest GetQueryableVotingMethods();
    }

    interface ICompetingCountryRankingsEndpoints
    {
        RestRequest GetCompetingCountryPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetCompetingCountryPointsConsensusRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetCompetingCountryPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetCompetingCountryPointsShareRankings(IReadOnlyDictionary<string, object?> queryParams);
    }

    interface ICompetitorRankingsEndpoints
    {
        RestRequest GetCompetitorPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetCompetitorPointsConsensusRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetCompetitorPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetCompetitorPointsShareRankings(IReadOnlyDictionary<string, object?> queryParams);
    }

    interface IVotingCountryRankingsEndpoints
    {
        RestRequest GetVotingCountryPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetVotingCountryPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetVotingCountryPointsShareRankings(IReadOnlyDictionary<string, object?> queryParams);
    }
}
