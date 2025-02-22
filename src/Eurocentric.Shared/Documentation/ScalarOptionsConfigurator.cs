using Eurocentric.Shared.Security;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

namespace Eurocentric.Shared.Documentation;

internal sealed class ScalarOptionsConfigurator : IConfigureOptions<ScalarOptions>
{
    public void Configure(ScalarOptions options) => options.WithTheme(ScalarTheme.Kepler)
        .WithTitle("API documentation")
        .WithPreferredScheme(ApiKeyAuthenticationHandler.SchemeName)
        .WithApiKeyAuthentication(apiKey => apiKey.Token = "YOUR_API_KEY");
}
