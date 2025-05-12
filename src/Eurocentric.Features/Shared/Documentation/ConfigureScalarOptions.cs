using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

namespace Eurocentric.Features.Shared.Documentation;

/// <summary>
///     Configures the Scalar documentation web page options for the application.
/// </summary>
internal sealed class ConfigureScalarOptions : IConfigureOptions<ScalarOptions>
{
    public void Configure(ScalarOptions options)
    {
        options.Theme = ScalarTheme.Kepler;
        options.Title = "API Documentation";
        options.HideModels = true;
        options.OperationSorter = OperationSorter.Method;
        options.TagSorter = TagSorter.Alpha;
        options.DefaultHttpClient = new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.JavaScript, ScalarClient.Fetch);
    }
}
