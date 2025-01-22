using RestSharp;

namespace Eurocentric.Tests.Utils.Fixtures;

public static class Requests
{
    public static class Get
    {
        public static RestRequest To(string route) => new RestRequest(route)
            .AddHeader("Accept", "application/json");
    }

    public static class Post
    {
        public static RestRequest To(string route) => new RestRequest(route, Method.Post)
            .AddHeader("Accept", "application/json")
            .AddHeader("Content-Type", "application/json");
    }
}
