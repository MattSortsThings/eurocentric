using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.PublicApi.V0;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the Public API version 0 documents to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddPublicApiV0OpenApiDocuments(this IServiceCollection services)
    {
        services.AddOpenApi("public-api-v0.1", options =>
        {
            options.ShouldInclude = apiDescription => apiDescription.GroupName == ApiNames.EndpointGroup &&
                                                      apiDescription.GetApiVersion() is { MajorVersion: 0, MinorVersion: 1 };

            options.AddDocumentTransformer(new InfoDocumentTransformer("Eurocentric Public API",
                "A web API for (over)analysing the Eurovision Song Contest, 2016-2025.",
                "v0.1"));
        });

        services.AddOpenApi("public-api-v0.2", options =>
        {
            options.ShouldInclude = apiDescription => apiDescription.GroupName == ApiNames.EndpointGroup &&
                                                      apiDescription.GetApiVersion() is { MajorVersion: 0, MinorVersion: 2 };

            options.AddDocumentTransformer(new InfoDocumentTransformer("Eurocentric Public API",
                "A web API for (over)analysing the Eurovision Song Contest, 2016-2025.",
                "v0.2"));
        });

        return services;
    }
}
