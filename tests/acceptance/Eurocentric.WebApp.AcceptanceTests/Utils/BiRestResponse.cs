using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.Utils;

public sealed class BiRestResponse
{
    private readonly RestResponse? _successfulResponse;
    private readonly RestResponse<ProblemDetails>? _unsuccessfulResponse;

    private BiRestResponse(RestResponse? successfulResponse = null,
        RestResponse<ProblemDetails>? unsuccessfulResponse = null)
    {
        _successfulResponse = successfulResponse;
        _unsuccessfulResponse = unsuccessfulResponse;
    }

    private bool IsSuccessful => _successfulResponse is not null;

    public RestResponse AsSuccessful => _successfulResponse ?? throw new InvalidOperationException("Instance is unsuccessful.");

    public RestResponse<ProblemDetails> AsUnsuccessful => _unsuccessfulResponse
                                                          ?? throw new InvalidOperationException("Instance is successful.");

    public void Switch(Action<RestResponse> onSuccessful, Action<RestResponse<ProblemDetails>> onUnsuccessful)
    {
        if (IsSuccessful)
        {
            onSuccessful(_successfulResponse!);
        }
        else
        {
            onUnsuccessful(_unsuccessfulResponse!);
        }
    }

    public static BiRestResponse Successful(RestResponse response) => new(response);

    public static BiRestResponse Unsuccessful(RestResponse<ProblemDetails> response) => new(unsuccessfulResponse: response);
}
