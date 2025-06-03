using Eurocentric.Features.AcceptanceTests.Utilities;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utilities;

public sealed partial class PublicApiV0Driver : IPublicApiV0Driver
{
    private readonly string _apiVersion;
    private readonly IWebAppFixtureRestClient _restClient;

    private PublicApiV0Driver(IWebAppFixtureRestClient restClient, string apiVersion)
    {
        _apiVersion = apiVersion;
        _restClient = restClient;
    }

    public IPublicApiV0Driver.IFilters Filters => this;

    public IPublicApiV0Driver.IVotingCountryRankings VotingCountryRankings => this;

    public static PublicApiV0Driver Create(IWebAppFixtureRestClient restClient, string apiVersion) =>
        new(restClient, apiVersion);

    private static RestRequest Get(string route) => new(route);
}
