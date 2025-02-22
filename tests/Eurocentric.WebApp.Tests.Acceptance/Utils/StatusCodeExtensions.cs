using System.Net;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

internal static class StatusCodeExtensions
{
    internal static void ShouldBe(this HttpStatusCode statusCode, HttpStatusCode expectedStatusCode) =>
        Assert.Equal(expectedStatusCode, statusCode);
}
