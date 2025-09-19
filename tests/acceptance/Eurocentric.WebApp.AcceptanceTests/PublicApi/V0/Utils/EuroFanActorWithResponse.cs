using Eurocentric.WebApp.AcceptanceTests.Utils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Utils;

public abstract class EuroFanActorWithResponse<TResponse> : ActorWithResponse<TResponse> where TResponse : class
{
    protected EuroFanActorWithResponse(IApiDriver apiDriver)
    {
        ApiDriver = apiDriver;
    }

    private protected IApiDriver ApiDriver { get; }

    public override async Task When_I_send_my_request()
    {
        RestRequest request = await Assert.That(Request).IsNotNull();

        BiRestResponse<TResponse> response =
            await ApiDriver.RestClient.SendAsync<TResponse>(request, TestContext.Current!.CancellationToken);

        response.Switch(OnSuccessful, OnUnsuccessful);
    }

    private void OnSuccessful(RestResponse<TResponse> response)
    {
        ResponseStatusCode = response.StatusCode;
        ResponseObject = response.Data;
    }

    private void OnUnsuccessful(RestResponse<ProblemDetails> response)
    {
        ResponseStatusCode = response.StatusCode;
        ResponseProblemDetails = response.Data;
    }
}
