using System.Net;
using RestSharp;

namespace Eurocentric.PublicApi.Tests.Acceptance.Utils;

internal static class RestResponseExtensions
{
    internal static void ShouldHaveStatusCode(this RestResponse response, HttpStatusCode expected) =>
        Assert.Equal(expected, response.StatusCode);
}
