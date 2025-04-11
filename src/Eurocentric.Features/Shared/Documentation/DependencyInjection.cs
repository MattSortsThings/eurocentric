using Eurocentric.Features.Shared.ApiDiscovery;
using Eurocentric.Features.Shared.Json;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.Shared.Documentation;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the OpenAPI documents and documentation services to the service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        services.ConfigureOptions<ConfigureJsonOptions>();

        using IServiceScope scope = services.BuildServiceProvider().CreateScope();

        IEnumerable<IApiInfo> apis = scope.ServiceProvider.GetRequiredService<IEnumerable<IApiInfo>>();

        foreach (IApiInfo api in apis)
        {
            foreach (var (name, major, minor) in api.Releases)
            {
                services.AddOpenApi(name, options =>
                {
                    options.AddDocumentTransformer((doc, _, _) =>
                    {
                        doc.Info.Title = api.OpenApiDocumentTitle;
                        doc.Info.Description = api.OpenApiDocumentDescription;
                        doc.Info.Version = $"{major}.{minor}";

                        return Task.CompletedTask;
                    });

                    options.ShouldInclude = description => description.GroupName == api.Name
                                                           && description.GetApiVersion() is { } apiVersion
                                                           && apiVersion.MajorVersion == major
                                                           && apiVersion.MinorVersion == minor;
                });
            }
        }

        return services;
    }
}
