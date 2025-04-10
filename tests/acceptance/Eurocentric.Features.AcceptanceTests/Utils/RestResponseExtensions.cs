using System.Net;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utils;

internal static class RestResponseExtensions
{
    internal static void Deconstruct<T>(this RestResponse<T> restResponse,
        out HttpStatusCode statusCode,
        out T responseObject,
        out IReadOnlyCollection<HeaderParameter> headers)
    {
        statusCode = restResponse.StatusCode;
        responseObject = restResponse.Data!;
        headers = restResponse.Headers!;
    }
}
