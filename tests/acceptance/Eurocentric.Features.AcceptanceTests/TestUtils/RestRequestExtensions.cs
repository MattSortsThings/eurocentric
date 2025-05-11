using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

internal static class RestRequestExtensions
{
    internal static RestRequest UseDemoApiKey(this RestRequest request) =>
        request.AddHeader("X-Api-Key", TestApiKeys.Demo);

    internal static RestRequest UseSecretApiKey(this RestRequest request) =>
        request.AddHeader("X-Api-Key", TestApiKeys.Secret);
}
