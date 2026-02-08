using System.Net;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Tests.Acceptance.PublicApi.V0.Utils;

public abstract class EuroFanExpecting200OK<TBody> : EuroFan
    where TBody : class
{
    protected EuroFanExpecting200OK(IEuroFanKernel kernel)
        : base(kernel) { }

    private protected RestResponse<TBody>? SuccessResponse { get; private set; }

    public override async Task When_I_send_my_request()
    {
        RestRequest request = await Assert.That(Request).IsNotNull();

        Result<RestResponse<TBody>, RestResponse<ProblemDetails>> response =
            await Kernel.RestClient.SendRequestAsync<TBody>(request);

        response.Match(
            successResponse => SuccessResponse = successResponse,
            failureResponse => FailureResponse = failureResponse
        );
    }

    public async Task Then_my_request_should_succeed_with_status_code_200_OK()
    {
        await Assert
            .That(SuccessResponse)
            .IsNotNull()
            .And.HasProperty(response => response.StatusCode, HttpStatusCode.OK);
    }
}
