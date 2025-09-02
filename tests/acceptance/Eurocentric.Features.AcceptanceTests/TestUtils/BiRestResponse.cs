using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public sealed class BiRestResponse
{
    private readonly RestResponse? _successfulResponse;
    private readonly RestResponse<ProblemDetails>? _unsuccessfulResponse;

    public BiRestResponse(RestResponse successfulResponse)
    {
        _successfulResponse = successfulResponse;
        _unsuccessfulResponse = null;
    }

    public BiRestResponse(RestResponse<ProblemDetails> unsuccessfulResponse)
    {
        _successfulResponse = null;
        _unsuccessfulResponse = unsuccessfulResponse;
    }

    public bool IsSuccessful => _successfulResponse is not null;

    public RestResponse AsSuccessful => _successfulResponse!;

    public RestResponse<ProblemDetails> AsUnsuccessful => _unsuccessfulResponse!;

    public void Switch(Action<RestResponse<ProblemDetails>> onUnsuccessful, Action<RestResponse> onSuccessful)
    {
        if (_successfulResponse is not null)
        {
            onSuccessful(_successfulResponse);
        }
        else
        {
            onUnsuccessful(_unsuccessfulResponse!);
        }
    }
}
