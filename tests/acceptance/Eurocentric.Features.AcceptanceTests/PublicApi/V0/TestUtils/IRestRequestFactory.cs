using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils;

public interface IRestRequestFactory
{
    ICompetingCountryRankingsEndpoints CompetingCountryRankings { get; }

    IQueryablesEndpoints Queryables { get; }

    interface ICompetingCountryRankingsEndpoints
    {
        RestRequest GetCompetingCountryPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams);
    }

    interface IQueryablesEndpoints
    {
        RestRequest GetQueryableContestStages();

        RestRequest GetQueryableCountries();

        RestRequest GetQueryableVotingMethods();
    }
}
