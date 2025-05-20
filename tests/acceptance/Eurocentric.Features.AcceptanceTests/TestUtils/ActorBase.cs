using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public abstract class ActorBase
{
    private protected HttpStatusCode StatusCode { get; set; }

    private protected ProblemDetails? ProblemDetails { get; set; }

    public void Then_my_request_should_succeed_with_status_code(HttpStatusCode expectedStatusCode)
    {
        Assert.InRange((int)StatusCode, 200, 299);
        Assert.Equal(expectedStatusCode, StatusCode);
    }

    public void Then_my_request_should_fail_with_status_code(HttpStatusCode expectedStatusCode)
    {
        Assert.InRange((int)StatusCode, 400, 499);
        Assert.Equal(expectedStatusCode, StatusCode);
    }

    public void Then_the_problem_details_should_match(string title = "Title", string detail = "Detail", int status = 400)
    {
        Assert.NotNull(ProblemDetails);
        Assert.Equal(title, ProblemDetails.Title);
        Assert.Equal(detail, ProblemDetails.Detail);
        Assert.Equal(status, ProblemDetails.Status);
    }

    public void Then_the_problem_details_extensions_should_contain(string key, string value)
    {
        Assert.NotNull(ProblemDetails);

        Assert.Contains(ProblemDetails.Extensions,
            kvp => kvp.Key == key && kvp.Value is JsonElement j && j.GetString() == value);
    }

    public void Then_the_problem_details_extensions_should_contain(string key, int value)
    {
        Assert.NotNull(ProblemDetails);

        Assert.Contains(ProblemDetails.Extensions,
            kvp => kvp.Key == key && kvp.Value is JsonElement j && j.GetInt32() == value);
    }
}
