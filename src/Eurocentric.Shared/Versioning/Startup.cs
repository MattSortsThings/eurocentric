using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.Versioning;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
internal static class Startup
{
    /// <summary>
    ///     Adds the API versioning services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "VV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}
