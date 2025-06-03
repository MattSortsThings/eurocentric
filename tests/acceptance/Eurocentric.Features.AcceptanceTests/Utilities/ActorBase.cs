using System.Net;
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
}
