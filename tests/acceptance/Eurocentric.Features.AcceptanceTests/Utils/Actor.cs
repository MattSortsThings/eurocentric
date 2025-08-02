using System.Net;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utils;

public abstract class Actor
{
    private protected RestRequest? Request { get; set; } = null!;

    private protected HttpStatusCode? ResponseStatusCode { get; set; }

    private protected ProblemDetails? ResponseProblemDetails { get; set; }

    public abstract Task When_I_send_my_request();

    public async Task Then_my_request_should_SUCCEED_with_status_code_200_OK() => await Assert.That(ResponseStatusCode)
        .IsEqualTo(HttpStatusCode.OK);

    public async Task Then_my_request_should_SUCCEED_with_status_code_201_OK() => await Assert.That(ResponseStatusCode)
        .IsEqualTo(HttpStatusCode.Created);

    public async Task Then_my_request_should_FAIL_with_status_code_404_NotFound() => await Assert.That(ResponseStatusCode)
        .IsEqualTo(HttpStatusCode.NotFound);

    public async Task Then_the_response_problem_details_should_match(string detail = "", string title = "", int status = 0) =>
        await Assert.That(ResponseProblemDetails)
            .IsNotNull()
            .And.HasMember(problemDetails => problemDetails.Detail).EqualTo(detail)
            .And.HasMember(problemDetails => problemDetails.Title).EqualTo(title)
            .And.HasMember(problemDetails => problemDetails.Status).EqualTo(status);
}
