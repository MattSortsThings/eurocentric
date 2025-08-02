using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

public interface IRestRequestFactory
{
    public IQueryablesEndpoints Queryables { get; }

    public IRankingsEndpoints Rankings { get; }

    public interface IQueryablesEndpoints
    {
        public RestRequest GetQueryableContestStages();

        public RestRequest GetQueryableCountries();
    }

    public interface IRankingsEndpoints
    {
        public RestRequest GetCompetingCountryPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams);

        public RestRequest GetCompetingCountryPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams);
    }
}
