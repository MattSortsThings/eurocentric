using System.Net;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utils;

public abstract class Actor<TResponse> where TResponse : class
{
    protected Actor(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor)
    {
        RestClient = restClient;
        BackDoor = backDoor;
    }

    private HttpStatusCode ResponseStatusCode { get; set; }

    public IWebAppFixtureRestClient RestClient { get; }

    public IWebAppFixtureBackDoor BackDoor { get; }

    public RestRequest? Request { get; set; }

    public ProblemDetails? ResponseProblemDetails { get; private set; }

    public TResponse? ResponseObject { get; private set; }

    public async Task When_I_send_my_request()
    {
        Assert.NotNull(Request);

        ProblemOrResponse<TResponse> problemOrResponse = await RestClient.SendAsync<TResponse>(Request);

        problemOrResponse.Switch(problem =>
        {
            ResponseStatusCode = problem.StatusCode;
            ResponseProblemDetails = problem.Data;
        }, response =>
        {
            ResponseStatusCode = response.StatusCode;
            ResponseObject = response.Data;
        });
    }

    public void Then_my_request_should_succeed_with_status_code(int statusCode)
    {
        int actualStatusCode = (int)ResponseStatusCode;

        Assert.InRange(actualStatusCode, 200, 299);
        Assert.Equal(statusCode, actualStatusCode);
    }

    public void Then_my_request_should_fail_with_status_code(int statusCode)
    {
        int actualStatusCode = (int)ResponseStatusCode;

        Assert.InRange(actualStatusCode, 400, 499);
        Assert.Equal(statusCode, actualStatusCode);
    }
}
