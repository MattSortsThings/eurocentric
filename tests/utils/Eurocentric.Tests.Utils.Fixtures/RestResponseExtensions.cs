using System.Net;
using RestSharp;

namespace Eurocentric.Tests.Utils.Fixtures;

public static class RestResponseExtensions
{
    public static void Deconstruct<T>(this RestResponse<T> response, out HttpStatusCode statusCode, out T responseObject)
    {
        responseObject = response.Data!;
        statusCode = response.StatusCode;
    }
}
