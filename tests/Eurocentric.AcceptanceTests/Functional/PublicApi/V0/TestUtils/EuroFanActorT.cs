using System.Net;
using Eurocentric.AcceptanceTests.TestUtils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V0.TestUtils;

public abstract class EuroFanActor<T> : IActor<T>
    where T : class
{
    /// <summary>
    ///     The kernel for all interactions with the system under test.
    /// </summary>
    private protected abstract EuroFanKernel Kernel { get; }

    /// <inheritdoc />
    public RestRequest? Request { get; private protected set; }

    /// <inheritdoc />
    public RestResponse<T>? SuccessResponse { get; private set; }

    /// <inheritdoc />
    public RestResponse<ProblemDetails>? FailureResponse { get; private set; }

    public async Task When_I_send_my_request()
    {
        RestRequest request = await Assert.That(Request).IsNotNull();

        ProblemOrResponse<T> problemOrResponse = await Kernel.Client.SendAsync<T>(
            request,
            TestContext.Current!.CancellationToken
        );

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

    private void OnProblem(RestResponse<ProblemDetails> problem) => FailureResponse = problem;

    private void OnResponse(RestResponse<T> response) => SuccessResponse = response;
}
