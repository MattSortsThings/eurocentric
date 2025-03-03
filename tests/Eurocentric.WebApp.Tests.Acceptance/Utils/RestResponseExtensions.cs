using System.Net;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

public static class RestResponseExtensions
{
    public static void Deconstruct<T>(this RestResponse<T> response, out HttpStatusCode statusCode, out T result)
    {
        statusCode = response.StatusCode;
        result = response.Data!;
    }

    public static void Deconstruct<T>(this RestResponse<T> response, out HttpStatusCode statusCode, out T result,
        out string location)
    {
        statusCode = response.StatusCode;
        result = response.Data!;
        location = response.Headers!.Single(header => header.Name == "Location").Value;
    }
}
