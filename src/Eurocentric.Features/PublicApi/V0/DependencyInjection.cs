using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.PublicApi.V0;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Registers the OpenAPI documents for the Public API Version 0 with the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection RegisterPublicApiV0OpenApiDocuments(this IServiceCollection services)
    {
        services.AddOpenApi("public-api-v0.1", options =>
        {
            options.ShouldInclude = description =>
                description.GroupName == "PublicApi.V0"
                && description.GetApiVersion() is { MajorVersion: 0, MinorVersion: 1 };
        });

        services.AddOpenApi("public-api-v0.2", options =>
        {
            options.ShouldInclude = description =>
                description.GroupName == "PublicApi.V0"
                && description.GetApiVersion() is { MajorVersion: 0, MinorVersion: 2 };
        });


        return services;
    }
}
