using RestSharp;

namespace Eurocentric.Tests.Acceptance.Utils;

public static class Actor
{
    public static ActorWithoutResponseBuilder WithoutResponse() => new();

    public static ActorWithResponseBuilder<TResponse> WithResponse<TResponse>()
        where TResponse : class => new();

    public sealed class ActorWithoutResponseBuilder
    {
        private WebApp SystemUnderTest { get; set; } = null!;

        private string ApiKey { get; set; } = string.Empty;

        private string ApiVersion { get; set; } = string.Empty;

        public ActorWithoutResponseBuilder Testing(WebApp systemUnderTest)
        {
            SystemUnderTest = systemUnderTest;

            return this;
        }

        public ActorWithoutResponseBuilder UsingDemoApiKey()
        {
            ApiKey = TestApiKeys.DemoApiKey;

            return this;
        }

        public ActorWithoutResponseBuilder UsingSecretApiKey()
        {
            ApiKey = TestApiKeys.SecretApiKey;

            return this;
        }

        public ActorWithoutResponseBuilder UsingApiVersion(string apiVersion)
        {
            ApiVersion = apiVersion;

            return this;
        }

        public TActor Build<TActor>(Func<IActorKernel, TActor> factory)
            where TActor : ActorWithoutResponse
        {
            RequestFactory requestFactory = new(ApiKey, ApiVersion);
            Kernel kernel = new(SystemUnderTest, requestFactory);

            return factory(kernel);
        }
    }

    public sealed class ActorWithResponseBuilder<TResponse>
        where TResponse : class
    {
        private WebApp SystemUnderTest { get; set; } = null!;

        private string ApiKey { get; set; } = string.Empty;

        private string ApiVersion { get; set; } = string.Empty;

        public ActorWithResponseBuilder<TResponse> Testing(WebApp systemUnderTest)
        {
            SystemUnderTest = systemUnderTest;

            return this;
        }

        public ActorWithResponseBuilder<TResponse> UsingDemoApiKey()
        {
            ApiKey = TestApiKeys.DemoApiKey;

            return this;
        }

        public ActorWithResponseBuilder<TResponse> UsingSecretApiKey()
        {
            ApiKey = TestApiKeys.SecretApiKey;

            return this;
        }

        public ActorWithResponseBuilder<TResponse> UsingApiVersion(string apiVersion)
        {
            ApiVersion = apiVersion;

            return this;
        }

        public TActor Build<TActor>(Func<IActorKernel, TActor> factory)
            where TActor : ActorWithResponse<TResponse>
        {
            RequestFactory requestFactory = new(ApiKey, ApiVersion);
            Kernel kernel = new(SystemUnderTest, requestFactory);

            return factory(kernel);
        }
    }

    private class Kernel(WebApp webApp, RequestFactory requestFactory) : IActorKernel
    {
        public IWebAppBackDoor BackDoor { get; } = webApp;

        public IWebAppRestClient RestClient { get; } = webApp;

        public IRestRequestFactory RestRequestFactory { get; } = requestFactory;
    }

    private class RequestFactory(string apiKey, string apiVersion) : IRestRequestFactory
    {
        public string ApiKey { get; } = apiKey;

        public string ApiVersion { get; } = apiVersion;

        public RestRequest GetRequest(string route) => new(route);

        public RestRequest DeleteRequest(string route) => new(route, Method.Delete);

        public RestRequest PatchRequest(string route) => new(route, Method.Patch);

        public RestRequest PostRequest(string route) => new(route, Method.Post);
    }
}
