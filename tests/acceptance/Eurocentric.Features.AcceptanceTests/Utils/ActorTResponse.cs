using System.Net;
using System.Text.Json;
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

    public void Then_the_response_problem_details_should_match(string detail = "", string title = "", int status = 0)
    {
        Assert.NotNull(ResponseProblemDetails);

        Assert.Equal(detail, ResponseProblemDetails.Detail);
        Assert.Equal(title, ResponseProblemDetails.Title);
        Assert.Equal(status, ResponseProblemDetails.Status);
    }

    public void Then_the_response_problem_details_extensions_should_contain(string value = "", string key = "")
    {
        Assert.NotNull(ResponseProblemDetails);

        Assert.Contains(ResponseProblemDetails.Extensions,
            kvp => kvp.Key == key && kvp.Value is JsonElement je && je.GetString() == value);
    }

    public void Then_the_response_problem_details_extensions_should_contain(int value = 0, string key = "")
    {
        Assert.NotNull(ResponseProblemDetails);

        Assert.Contains(ResponseProblemDetails.Extensions,
            kvp => kvp.Key == key && kvp.Value is JsonElement je && je.GetInt32() == value);
    }
}
