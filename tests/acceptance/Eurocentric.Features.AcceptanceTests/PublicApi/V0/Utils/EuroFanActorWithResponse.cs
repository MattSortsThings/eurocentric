using Eurocentric.Features.AcceptanceTests.Utils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

public abstract class EuroFanActorWithResponse<TResponse> : Actor
    where TResponse : class
{
    protected EuroFanActorWithResponse(IApiDriver apiDriver)
    {
        ApiDriver = apiDriver;
    }

    private protected IApiDriver ApiDriver { get; }

    private protected TResponse? ResponseBody { get; private set; }

    public override async Task When_I_send_my_request()
    {
        RestRequest request = await Assert.That(Request).IsNotNull();

        ProblemOrResponse<TResponse> problemOrResponse = await ApiDriver.RestClient.SendAsync<TResponse>(request);

        problemOrResponse.Switch(PopulateFromUnsuccessfulResponse, PopulateFromSuccessfulResponse);
    }

    private void PopulateFromUnsuccessfulResponse(RestResponse<ProblemDetails> r)
    {
        ResponseStatusCode = r.StatusCode;
        ResponseProblemDetails = r.Data;
    }

    private void PopulateFromSuccessfulResponse(RestResponse<TResponse> r)
    {
        ResponseStatusCode = r.StatusCode;
        ResponseBody = r.Data;
    }
}
