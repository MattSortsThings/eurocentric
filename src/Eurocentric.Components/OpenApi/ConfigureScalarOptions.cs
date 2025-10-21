using Eurocentric.Components.Security;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

namespace Eurocentric.Components.OpenApi;

/// <summary>
///     Configures the Scalar documentation web page options for the application.
/// </summary>
public sealed class ConfigureScalarOptions : IConfigureOptions<ScalarOptions>
{
    public void Configure(ScalarOptions options)
    {
        options.Theme = ScalarTheme.Kepler;
        options.Title = "API Documentation";
        options.HideModels = true;
        options.OperationSorter = OperationSorter.Method;
        options.TagSorter = TagSorter.Alpha;
        options.DefaultHttpClient = new KeyValuePair<ScalarTarget, ScalarClient>(
            ScalarTarget.CSharp,
            ScalarClient.RestSharp
        );
        options.AddPreferredSecuritySchemes(AuthenticationConstants.SchemeName);
        options.AddApiKeyAuthentication(
            AuthenticationConstants.SchemeName,
            scheme => scheme.Name = AuthenticationConstants.HttpRequestHeaderKey
        );
    }
}
