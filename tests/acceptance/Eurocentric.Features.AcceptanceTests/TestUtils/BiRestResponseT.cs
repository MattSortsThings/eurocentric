using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public sealed class BiRestResponse<T> where T : class
{
    private readonly RestResponse<T>? _successfulResponse;
    private readonly RestResponse<ProblemDetails>? _unsuccessfulResponse;

    public BiRestResponse(RestResponse<T> successfulResponse)
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

    public RestResponse<T> AsSuccessful => _successfulResponse!;

    public RestResponse<ProblemDetails> AsUnsuccessful => _unsuccessfulResponse!;

    public void Switch(Action<RestResponse<ProblemDetails>> onUnsuccessful, Action<RestResponse<T>> onSuccessful)
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
