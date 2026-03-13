using System.Text.Json;
using System.Text.Json.Serialization;

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
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.WriteIndented = false;
        });

        return builder;
    }
}
