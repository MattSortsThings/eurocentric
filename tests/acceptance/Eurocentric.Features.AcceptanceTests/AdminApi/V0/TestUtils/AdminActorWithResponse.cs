using Eurocentric.Features.AcceptanceTests.TestUtils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils;

public abstract class AdminActorWithResponse<TResponse>(IApiDriver apiDriver) : Actor
    where TResponse : class
{
    private protected IApiDriver ApiDriver { get; } = apiDriver;

    private protected TResponse? ResponseBody { get; private set; }

    public override async Task When_I_send_my_request()
    {
        RestRequest request = await Assert.That(Request).IsNotNull();

        BiRestResponse<TResponse> response = await ApiDriver.RestClient.SendAsync<TResponse>(request,
            TestContext.Current!.CancellationToken);

        response.Switch(PopulateFromUnsuccessfulResponse, PopulateFromSuccessfulResponse);
    }

    private void PopulateFromUnsuccessfulResponse(RestResponse<ProblemDetails> response)
    {
        ResponseStatusCode = response.StatusCode;
        ResponseProblemDetails = response.Data;
    }

    private void PopulateFromSuccessfulResponse(RestResponse<TResponse> response)
    {
        ResponseStatusCode = response.StatusCode;
        ResponseBody = response.Data;
    }
}
