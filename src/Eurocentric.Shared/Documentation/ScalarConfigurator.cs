using Eurocentric.Shared.Security;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

namespace Eurocentric.Shared.Documentation;

internal sealed class ScalarConfigurator : IConfigureOptions<ScalarOptions>
{
    public void Configure(ScalarOptions options) => options.WithTitle("API documentation")
        .WithTheme(ScalarTheme.Kepler)
        .WithModels(false)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.RestSharp)
        .WithPreferredScheme(ApiKeyConstants.ApiKeySchemeName)
        .WithApiKeyAuthentication(apiKey => apiKey.Token = "YOUR_API_KEY");
}
