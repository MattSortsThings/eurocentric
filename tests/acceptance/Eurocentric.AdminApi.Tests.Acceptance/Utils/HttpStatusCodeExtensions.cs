using System.Net;

namespace Eurocentric.PublicApi.Tests.Acceptance.Utils;

internal static class HttpStatusCodeExtensions
{
    internal static void ShouldBe(this HttpStatusCode httpStatusCode, HttpStatusCode expected) =>
        Assert.Equal(expected, httpStatusCode);
}
