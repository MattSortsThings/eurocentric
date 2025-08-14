using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AcceptanceTests.Utils.Assertions;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;

public abstract class EuroFanActorWithResponse<TResponse>(IApiDriver apiDriver) : Actor where TResponse : class
{
    private protected IApiDriver ApiDriver { get; } = apiDriver;

    private protected TResponse? ResponseBody { get; private set; }

    public override async Task When_I_send_my_request()
    {
        RestRequest request = await Assert.That(Request).IsNotNull();

        ProblemOrResponse<TResponse> problemOrResponse = await ApiDriver.RestClient.SendAsync<TResponse>(request);

        problemOrResponse.Switch(PopulateFromUnsuccessfulResponse, PopulateFromSuccessfulResponse);
    }

    public async Task Then_the_response_problem_details_extensions_should_include(string key, int value) =>
        await Assert.That(ResponseProblemDetails).IsNotNull().And.HasExtension(key, value);

    public async Task Then_the_response_problem_details_extensions_should_include(string key, string value) =>
        await Assert.That(ResponseProblemDetails).IsNotNull().And.HasExtension(key, value);

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
