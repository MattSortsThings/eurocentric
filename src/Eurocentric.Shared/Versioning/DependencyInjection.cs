using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.Versioning;

/// <summary>
///     Extension methods to be invoked at application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the API versioning services to the service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}
