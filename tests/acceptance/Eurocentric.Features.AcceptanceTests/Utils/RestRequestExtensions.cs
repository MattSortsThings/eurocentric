using Eurocentric.TestUtils.WebAppFixtures;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utils;

internal static class RestRequestExtensions
{
    internal static RestRequest UseSecretApiKey(this RestRequest request) => request.AddHeader("X-Api-Key", TestApiKeys.Secret);

    internal static RestRequest UseDemoApiKey(this RestRequest request) => request.AddHeader("X-Api-Key", TestApiKeys.Demo);
}
