using System.Net;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.Utils;

public abstract class Actor
{
    public RestRequest? Request { get; private protected set; } = null!;

    public HttpStatusCode? ResponseStatusCode { get; private protected set; }

    public ProblemDetails? ResponseProblemDetails { get; private protected set; }

    public abstract Task When_I_send_my_request();

    public async Task Then_my_request_should_FAIL_with_status_code_400_BadRequest() =>
        await Assert.That(ResponseStatusCode).IsEqualTo(HttpStatusCode.BadRequest);

    public async Task Then_my_request_should_FAIL_with_status_code_404_NotFound() =>
        await Assert.That(ResponseStatusCode).IsEqualTo(HttpStatusCode.NotFound);

    public async Task Then_my_request_should_FAIL_with_status_code_409_Conflict() =>
        await Assert.That(ResponseStatusCode).IsEqualTo(HttpStatusCode.Conflict);

    public async Task Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity() =>
        await Assert.That(ResponseStatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
}
