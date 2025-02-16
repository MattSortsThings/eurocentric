using System.Net;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

internal static class RestResponseExtensions
{
    internal static void Deconstruct<T>(this RestResponse<T> restResponse, out HttpStatusCode statusCode, out T result)
    {
        statusCode = restResponse.StatusCode;
        result = restResponse.Data!;
    }

    internal static void Deconstruct(this RestResponse restResponse, out HttpStatusCode statusCode, out string? content)
    {
        statusCode = restResponse.StatusCode;
        content = restResponse.Content;
    }
}
