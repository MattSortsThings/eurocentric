using RestSharp;

namespace Eurocentric.Tests.Utils.Fixtures;

public sealed class GetRequest : RestRequest
{
    private GetRequest(string route) : base(route) { }

    public static GetRequest To(string route) => new(route);
}
