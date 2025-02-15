using System.Net;
using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.Utils;

internal static class RestResponseExtensions
{
    internal static void Deconstruct<T>(this RestResponse<T> restResponse, out HttpStatusCode statusCode, out T result)
    {
        string? p = restResponse.Content;

        statusCode = restResponse.StatusCode;
        result = restResponse.Data!;
    }
}
