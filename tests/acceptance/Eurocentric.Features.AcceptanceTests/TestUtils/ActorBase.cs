using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public abstract class ActorBase<TResponse>
    where TResponse : class
{
    private HttpStatusCode StatusCode { get; set; }

    private ProblemDetails? ProblemDetails { get; set; }

    private protected TResponse? Response { get; set; }

    private protected abstract Func<Task<ResponseOrProblem<TResponse>>> SendMyRequest { get; set; }

    public async Task When_I_send_my_request()
    {
        ResponseOrProblem<TResponse> responseOrProblem = await SendMyRequest();

        responseOrProblem.Switch(success =>
        {
            StatusCode = success.StatusCode;
            Response = success.Data;
        }, problem =>
        {
            StatusCode = problem.StatusCode;
            ProblemDetails = problem.Data;
        });
    }

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
}
