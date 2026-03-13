using Eurocentric.Components.Configuration;
using Eurocentric.Components.DataAccess;

namespace Eurocentric.WebApp.Startup;

/// <summary>
///     Extension methods for the <see cref="WebApplicationBuilder" /> class.
/// </summary>
internal static class WebApplicationBuilderExtensions
{
    /// <summary>
    ///     Configures all the services for the web application to be built.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The original <see cref="WebApplicationBuilder" /> instance.</returns>
    public static WebApplicationBuilder ConfigureAllServices(this WebApplicationBuilder builder)
    {
        builder
            .Services.AddDataAccess()
            .ConfigureOptions<ConfigureDbConnectionOptions>()
            .ConfigureOptions<ConfigureHttpJsonOptions>();

        return builder;
    }
}
