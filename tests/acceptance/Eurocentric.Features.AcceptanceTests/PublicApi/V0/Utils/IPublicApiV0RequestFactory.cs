using Eurocentric.Features.PublicApi.V0.Rankings;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

public interface IPublicApiV0RequestFactory
{
    public IFiltersEndpoints Filters { get; }

    public IRankingsEndpoints Rankings { get; }

    public interface IFiltersEndpoints
    {
        public RestRequest GetContestStages();
        public RestRequest GetCountries();
        public RestRequest GetVotingMethods();
    }

    public interface IRankingsEndpoints
    {
        public RestRequest GetCompetingCountryPointsAverageRankings(GetCompetingCountryPointsAverageRankingsRequest query);
        public RestRequest GetCompetingCountryPointsInRangeRankings(GetCompetingCountryPointsInRangeRankingsRequest query);
    }
}
