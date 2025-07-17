using System.Net;
using Eurocentric.Features.AcceptanceTests.Utils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public abstract class AdminActorWithoutResponse : IAdminActor, IResponseVerifier
{
    private readonly Lazy<BroadcastCollection> _lazyBroadcastCollection = new(() => new BroadcastCollection());

    private readonly Lazy<ContestCollection> _lazyContestCollection = new(() => new ContestCollection());

    private readonly Lazy<CountryCollection> _lazyCountryCollection = new(() => new CountryCollection());

    public AdminActorWithoutResponse(IWebAppFixtureRestClient restClient,
        IWebAppFixtureBackDoor backDoor,
        IRequestFactory requestFactory)
    {
        RestClient = restClient;
        RequestFactory = requestFactory;
        BackDoor = backDoor;
    }

    public RestRequest? Request { get; set; }

    public IWebAppFixtureRestClient RestClient { get; }

    public IRequestFactory RequestFactory { get; }

    public IWebAppFixtureBackDoor BackDoor { get; }

    public BroadcastCollection GivenBroadcasts => _lazyBroadcastCollection.Value;

    public ContestCollection GivenContests => _lazyContestCollection.Value;

    public CountryCollection GivenCountries => _lazyCountryCollection.Value;

    public HttpStatusCode ResponseStatusCode { get; private set; }

    public ProblemDetails? ResponseProblemDetails { get; private set; }

    public async Task When_I_send_my_request()
    {
        Assert.NotNull(Request);

        ProblemOrResponse responseOrProblem = await RestClient.SendAsync(Request, TestContext.Current.CancellationToken);

        responseOrProblem.Switch(problem =>
        {
            ResponseStatusCode = problem.StatusCode;
            ResponseProblemDetails = problem.Data;
        }, response =>
        {
            ResponseStatusCode = response.StatusCode;
        });
    }
}
