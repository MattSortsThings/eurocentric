using Eurocentric.Features.Shared.Security;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

namespace Eurocentric.Features.Shared.Documentation;

internal sealed class ConfigureScalarOptions : IConfigureOptions<ScalarOptions>
{
    public void Configure(ScalarOptions options) => options.WithTheme(ScalarTheme.Solarized)
        .WithTitle("API documentation")
        .WithModels(false)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.RestSharp)
        .WithPreferredScheme(ApiKeyAuthenticationScheme.SchemeName)
        .WithApiKeyAuthentication(apiKey => apiKey.Token = "YOUR_API_KEY");
}
