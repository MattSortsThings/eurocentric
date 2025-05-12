using System.Net;
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
}
