using RestSharp;

namespace Eurocentric.AcceptanceTests.TestUtils.Contracts;

public static class RestRequests
{
    public static RestRequest Get(string route) => new(route);
}
