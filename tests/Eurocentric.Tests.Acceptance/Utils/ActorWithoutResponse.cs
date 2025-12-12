using System.Net;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Tests.Acceptance.Utils;

public abstract class ActorWithoutResponse
{
    protected ActorWithoutResponse(IActorKernel kernel)
    {
        Kernel = kernel;
    }

    private protected IActorKernel Kernel { get; }

    private protected RestRequest? Request { get; set; }

    private protected RestResponse? SuccessResponse { get; private set; }

    private protected RestResponse<ProblemDetails>? FailureResponse { get; private set; }

    public async Task When_I_send_my_request()
    {
        RestRequest request = await Assert.That(Request).IsNotNull();

        UnionRestResponse response = await Kernel.RestClient.SendRequestAsync(request);

        if (response.IsSuccessful)
        {
            SuccessResponse = response.AsSuccessful();
        }
        else
        {
            FailureResponse = response.AsUnsuccessful();
        }
    }

    public async Task Then_my_request_should_SUCCEED_with_status_code(int statusCode)
    {
        HttpStatusCode expectedStatusCode = (HttpStatusCode)statusCode;

        await Assert.That(SuccessResponse?.StatusCode).IsEqualTo(expectedStatusCode);
    }

    public async Task Then_my_request_should_FAIL_with_status_code(int statusCode)
    {
        HttpStatusCode expectedStatusCode = (HttpStatusCode)statusCode;

        await Assert.That(FailureResponse?.StatusCode).IsEqualTo(expectedStatusCode);
    }
}
