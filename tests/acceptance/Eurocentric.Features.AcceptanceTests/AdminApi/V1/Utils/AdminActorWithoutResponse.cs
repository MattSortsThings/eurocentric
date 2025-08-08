using Eurocentric.Features.AcceptanceTests.Utils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public abstract class AdminActorWithoutResponse(IApiDriver apiDriver) : Actor
{
    private protected IApiDriver ApiDriver { get; } = apiDriver;

    public override async Task When_I_send_my_request()
    {
        RestRequest request = await Assert.That(Request).IsNotNull();

        ProblemOrResponse problemOrResponse = await ApiDriver.RestClient.SendAsync(request);

        problemOrResponse.Switch(PopulateFromUnsuccessfulResponse, PopulateFromSuccessfulResponse);
    }

    private void PopulateFromUnsuccessfulResponse(RestResponse<ProblemDetails> r)
    {
        ResponseStatusCode = r.StatusCode;
        ResponseProblemDetails = r.Data;
    }

    private void PopulateFromSuccessfulResponse(RestResponse r) => ResponseStatusCode = r.StatusCode;
}
