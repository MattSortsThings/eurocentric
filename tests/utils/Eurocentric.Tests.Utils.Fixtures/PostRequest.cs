using RestSharp;

namespace Eurocentric.Tests.Utils.Fixtures;

public sealed class PostRequest : RestRequest
{
    private PostRequest(string route) : base(route, Method.Post) { }

    public static PostRequest To(string route) => new(route);
}
