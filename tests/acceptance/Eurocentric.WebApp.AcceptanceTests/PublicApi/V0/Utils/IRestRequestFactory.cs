using Eurocentric.Apis.Public.V0.Enums;
using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Utils;

public interface IRestRequestFactory
{
    ICompetingCountryRankingsEndpoints CompetingCountryRankings { get; }

    IQueryablesEndpoints Queryables { get; }

    IScoreboardsEndpoints Scoreboards { get; }

    interface IQueryablesEndpoints
    {
        RestRequest GetQueryableBroadcasts();

        RestRequest GetQueryableContests();

        RestRequest GetQueryableCountries();
    }

    interface IScoreboardsEndpoints
    {
        RestRequest GetScoreboard(int contestYear, ContestStage contestStage);
    }

    interface ICompetingCountryRankingsEndpoints
    {
        RestRequest GetCompetingCountryPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams);
    }
}
