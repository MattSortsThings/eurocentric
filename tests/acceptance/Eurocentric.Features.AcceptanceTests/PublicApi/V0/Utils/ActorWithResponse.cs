using System.Net;
using Eurocentric.Features.AcceptanceTests.Utils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

public abstract class ActorWithResponse<T> : IActor
{
    protected ActorWithResponse(IWebAppFixtureBackDoor backDoor, IWebAppFixtureRestClient restClient, string apiVersion = "v1.0")
    {
        BackDoor = backDoor;
        RestClient = restClient;
        ApiVersion = apiVersion;
    }

    private HttpStatusCode ResponseStatusCode { get; set; }

    private protected ProblemDetails? ResponseProblemDetails { get; set; }

    public T? ResponseObject { get; private set; }

    public IWebAppFixtureBackDoor BackDoor { get; }

    public IWebAppFixtureRestClient RestClient { get; }

    public string ApiVersion { get; }

    public RestRequest? Request { get; set; }

    public async Task When_I_send_my_request()
    {
        Assert.NotNull(Request);

        ProblemOrResponse<T> problemOrResponse =
            await RestClient.SendRequestAsync<T>(Request, TestContext.Current.CancellationToken);

        problemOrResponse.Switch(unsuccessfulResponse =>
            {
                ResponseProblemDetails = unsuccessfulResponse.Data;
                ResponseStatusCode = unsuccessfulResponse.StatusCode;
            }, successfulResponse =>
            {
                ResponseObject = successfulResponse.Data;
                ResponseStatusCode = successfulResponse.StatusCode;
            }
        );
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
