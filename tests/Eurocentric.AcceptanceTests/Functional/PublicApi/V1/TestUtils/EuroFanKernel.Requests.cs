using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;

public sealed partial class EuroFanKernel
{
    private sealed partial class RestRequestFactory(string apiVersion) : IRestRequestFactory
    {
        public IRestRequestFactory.ICompetingCountryRankingsEndpoints CompetingCountryRankings => this;

        public IRestRequestFactory.IQueryablesEndpoints Queryables => this;

        public IRestRequestFactory.IVotingCountryRankingsEndpoints VotingCountryRankings => this;

        private static RestRequest GetRequest(string route) => new(route);
    }
}
