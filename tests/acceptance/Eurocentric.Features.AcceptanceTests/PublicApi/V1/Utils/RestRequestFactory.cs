using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;

public sealed partial class RestRequestFactory : IRestRequestFactory
{
    private readonly string _apiVersion;

    public RestRequestFactory(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public IRestRequestFactory.ICompetingCountryRankingsEndpoints CompetingCountryRankings => this;

    public IRestRequestFactory.ICompetitorRankingsEndpoints CompetitorRankings => this;

    public IRestRequestFactory.IQueryablesEndpoints Queryables => this;

    public IRestRequestFactory.IVotingCountryRankingsEndpoints VotingCountryRankings => this;

    private static RestRequest GetRequest(string route) => new RestRequest(route).UseDemoApiKey();
}
