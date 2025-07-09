using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utils;

internal static class RestRequestExtensions
{
    internal static RestRequest UseDemoApiKey(this RestRequest restRequest)
    {
        restRequest.AddHeader("X-Api-Key", TestApiKeys.DemoApiKey);

        return restRequest;
    }

    internal static RestRequest UseSecretApiKey(this RestRequest restRequest)
    {
        restRequest.AddHeader("X-Api-Key", TestApiKeys.SecretApiKey);

        return restRequest;
    }
}
