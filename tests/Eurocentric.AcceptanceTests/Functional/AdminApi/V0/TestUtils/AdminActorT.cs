using System.Net;
using Eurocentric.AcceptanceTests.TestUtils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V0.TestUtils;

public abstract class AdminActor<T> : IActor<T>
    where T : class
{
    private protected abstract AdminKernel Kernel { get; }

    public RestRequest? Request { get; private protected set; }

    public RestResponse<T>? SuccessResponse { get; private set; }

    public RestResponse<ProblemDetails>? FailureResponse { get; private set; }

    public async Task When_I_send_my_request()
    {
        RestRequest request = await Assert.That(Request).IsNotNull();

        ProblemOrResponse<T> problemOrResponse = await Kernel.Client.SendAsync<T>(request);

        problemOrResponse.Switch(OnProblem, OnResponse);
    }

    public async Task Then_my_request_should_SUCCEED_with_status_code(int statusCode)
    {
        HttpStatusCode expectedStatusCode = (HttpStatusCode)statusCode;

        await Assert
            .That(SuccessResponse)
            .IsNotNull()
            .And.Member(response => response.StatusCode, assertion => assertion.IsEqualTo(expectedStatusCode));
    }

    public async Task Then_my_request_should_FAIL_with_status_code(int statusCode)
    {
        HttpStatusCode expectedStatusCode = (HttpStatusCode)statusCode;

        await Assert
            .That(FailureResponse)
            .IsNotNull()
            .And.Member(response => response.StatusCode, assertion => assertion.IsEqualTo(expectedStatusCode));
    }

    public async Task Then_the_response_problem_details_should_match(
        string detail = "",
        string title = "",
        int status = 0
    )
    {
        await Assert
            .That(FailureResponse?.Data)
            .IsNotNull()
            .And.HasTitle(title)
            .And.HasDetail(detail)
            .And.HasStatus(status);
    }

    public async Task Then_the_response_problem_details_extensions_should_include(int value = 0, string key = "") =>
        await Assert.That(FailureResponse?.Data).IsNotNull().And.HasExtension(key, value);

    public async Task Then_the_response_problem_details_extensions_should_include(string value = "", string key = "") =>
        await Assert.That(FailureResponse?.Data).IsNotNull().And.HasExtension(key, value);

    private void OnProblem(RestResponse<ProblemDetails> problem) => FailureResponse = problem;

    private void OnResponse(RestResponse<T> response) => SuccessResponse = response;
}
