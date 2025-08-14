using System.Net;
using Eurocentric.Features.AcceptanceTests.Utils.Assertions;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utils;

public abstract class Actor
{
    private protected RestRequest? Request { get; set; } = null!;

    private protected HttpStatusCode? ResponseStatusCode { get; set; }

    private protected ProblemDetails? ResponseProblemDetails { get; set; }

    public abstract Task When_I_send_my_request();

    public async Task Then_my_request_should_SUCCEED_with_status_code_200_OK() =>
        await Assert.That(ResponseStatusCode).IsEqualTo(HttpStatusCode.OK);

    public async Task Then_my_request_should_SUCCEED_with_status_code_201_Created() =>
        await Assert.That(ResponseStatusCode).IsEqualTo(HttpStatusCode.Created);

    public async Task Then_my_request_should_SUCCEED_with_status_code_204_NoContent() =>
        await Assert.That(ResponseStatusCode).IsEqualTo(HttpStatusCode.NoContent);

    public async Task Then_my_request_should_FAIL_with_status_code_400_BadRequest() =>
        await Assert.That(ResponseStatusCode).IsEqualTo(HttpStatusCode.BadRequest);

    public async Task Then_my_request_should_FAIL_with_status_code_404_NotFound() =>
        await Assert.That(ResponseStatusCode).IsEqualTo(HttpStatusCode.NotFound);

    public async Task Then_my_request_should_FAIL_with_status_code_409_Conflict() =>
        await Assert.That(ResponseStatusCode).IsEqualTo(HttpStatusCode.Conflict);

    public async Task Then_my_request_should_FAIL_with_status_code_422_UnprocessableEntity() =>
        await Assert.That(ResponseStatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);

    public async Task Then_the_response_problem_details_should_match(string detail = "", string title = "", int status = 0) =>
        await Assert.That(ResponseProblemDetails)
            .IsNotNull()
            .And.HasDetail(detail)
            .And.HasTitle(title)
            .And.HasStatus(status);
}
