using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.AcceptanceTests.Utilities;

public abstract class ActorBase
{
    private protected HttpStatusCode ResponseStatusCode { get; set; }

    private protected ProblemDetails? ResponseProblemDetails { get; set; }

    public abstract Task When_I_send_my_request();

    public void Then_my_request_should_succeed_with_status_code(HttpStatusCode statusCode)
    {
        Assert.InRange((int)ResponseStatusCode, 200, 299);
        Assert.Equal(statusCode, ResponseStatusCode);
    }

    public void Then_my_request_should_fail_with_status_code(HttpStatusCode statusCode)
    {
        Assert.InRange((int)ResponseStatusCode, 400, 499);
        Assert.Equal(statusCode, ResponseStatusCode);
    }

    public void Then_the_problem_details_should_match(string title = "", string detail = "", int status = 0)
    {
        Assert.NotNull(ResponseProblemDetails);

        Assert.Equal(title, ResponseProblemDetails.Title);
        Assert.Equal(detail, ResponseProblemDetails.Detail);
        Assert.Equal(status, ResponseProblemDetails.Status);
    }

    public void Then_the_problem_details_extensions_should_contain(string key, Guid value)
    {
        Assert.NotNull(ResponseProblemDetails);

        Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp.Key == key
                                                                  && kvp.Value is JsonElement e
                                                                  && e.GetGuid() == value);
    }
}
