using Eurocentric.Apis.Admin;
using Eurocentric.Apis.Public;
using Eurocentric.Components.Configuration;
using Eurocentric.Components.DataAccess;
using Eurocentric.Components.Messaging;

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
            .AddInternalMessagingPipeline(typeof(AdminApiEndpoints).Assembly, typeof(PublicApiEndpoints).Assembly)
            .ConfigureOptions<ConfigureDbConnectionOptions>()
            .ConfigureOptions<ConfigureHttpJsonOptions>();

        return builder;
    }
}
