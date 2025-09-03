using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Common.OpenApi;
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
    ///     Adds the OpenAPI documents for the Public API v0.x releases to the application service descriptor collection.
    /// </summary>
    /// <param name="services">The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</param>
    internal static void AddV0OpenApiDocuments(this IServiceCollection services)
    {
        services.AddOpenApi("public-api-v0.1", options =>
        {
            options.ShouldInclude = description =>
                description.GroupName == EndpointNames.Group
                && description.GetApiVersion() is { MajorVersion: 0, MinorVersion: 1 };

            options.AddDocumentTransformer<ApiKeySecurityDocumentTransformer>()
                .AddDocumentTransformer<V0Point1ApiReleaseDocumentTransformer>()
                .AddDocumentTransformer<V0Point1ApiInfoDocumentTransformer>();
        });

        services.AddOpenApi("public-api-v0.2", options =>
        {
            options.ShouldInclude = description =>
                description.GroupName == EndpointNames.Group
                && description.GetApiVersion() is { MajorVersion: 0, MinorVersion: 2 };

            options.AddDocumentTransformer<ApiKeySecurityDocumentTransformer>()
                .AddDocumentTransformer<V0Point2ApiReleaseDocumentTransformer>()
                .AddDocumentTransformer<V0Point2ApiInfoDocumentTransformer>();
        });
    }
}
