using Microsoft.AspNetCore.Builder;
using Scalar.AspNetCore;

namespace Eurocentric.Shared.OpenApi;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Configuration
{
    public static void UseDocumentationPages(this WebApplication app) =>
        app.MapScalarApiReference("docs", options =>
        {
            options.Theme = ScalarTheme.Kepler;
            options.DefaultHttpClient =
                new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.CSharp, ScalarClient.RestSharp);
        });
}
