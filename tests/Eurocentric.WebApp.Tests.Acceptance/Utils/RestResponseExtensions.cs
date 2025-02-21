using System.Net;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

internal static class RestResponseExtensions
{
    internal static void Deconstruct<T>(this RestResponse<T> response, out HttpStatusCode statusCode, out T result)
    {
        statusCode = response.StatusCode;
        result = response.Data!;
    }

    internal static void Deconstruct<T>(this RestResponse<T> response, out HttpStatusCode statusCode, out T result,
        out string location)
    {
        statusCode = response.StatusCode;
        result = response.Data!;
        location = response.Headers!.First(h => h.Name == "Location")!.Value;
    }
}
