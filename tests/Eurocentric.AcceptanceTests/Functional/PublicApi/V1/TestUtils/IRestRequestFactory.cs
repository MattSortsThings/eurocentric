using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;

public interface IRestRequestFactory
{
    ICompetingCountryRankingsEndpoints CompetingCountryRankings { get; }

    ICompetitorRankingsEndpoints CompetitorRankings { get; }

    IQueryablesEndpoints Queryables { get; }

    IVotingCountryRankingsEndpoints VotingCountryRankings { get; }

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

    interface IQueryablesEndpoints
    {
        RestRequest GetQueryableBroadcasts();

        RestRequest GetQueryableContests();

        RestRequest GetQueryableCountries();
    }

    interface IVotingCountryRankingsEndpoints
    {
        RestRequest GetVotingCountryPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetVotingCountryPointsConsensusRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetVotingCountryPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams);

        RestRequest GetVotingCountryPointsShareRankings(IReadOnlyDictionary<string, object?> queryParams);
    }
}
