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
    ///     Adds the OpenAPI documents for version 0 of the Public API to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddPublicApiV0OpenApiDocuments(this IServiceCollection services)
    {
        services.AddOpenApi(OpenApiDocumentNames.PublicApiV0Point1, options =>
        {
            options.ShouldInclude = description =>
                description.GroupName == ApiRelease.EndpointGroupName
                && description.GetApiVersion() is { MajorVersion: 0, MinorVersion: 1 };

            options.AddDocumentTransformer<ApiKeySecurityDocumentTransformer>()
                .AddOperationTransformer<ParameterExampleOperationTransformer>()
                .AddSchemaTransformer<ExampleSchemaTransformer>()
                .AddOperationTransformer<ProblemDetailsResponseExampleOperationTransformer>()
                .AddDocumentTransformer(new InfoDocumentTransformer("Eurocentric Public API",
                    "A web API for (over)analysing the Eurovision Song Contest, 2016-2025."));
        });

        services.AddOpenApi(OpenApiDocumentNames.PublicApiV0Point2, options =>
        {
            options.ShouldInclude = description =>
                description.GroupName == ApiRelease.EndpointGroupName
                && description.GetApiVersion() is { MajorVersion: 0, MinorVersion: 2 };

            options.AddDocumentTransformer<ApiKeySecurityDocumentTransformer>()
                .AddOperationTransformer<ParameterExampleOperationTransformer>()
                .AddSchemaTransformer<ExampleSchemaTransformer>()
                .AddOperationTransformer<ProblemDetailsResponseExampleOperationTransformer>()
                .AddDocumentTransformer(new InfoDocumentTransformer("Eurocentric Admin API",
                    "A web API for (over)analysing the Eurovision Song Contest, 2016-2025."));
        });

        return services;
    }
}
