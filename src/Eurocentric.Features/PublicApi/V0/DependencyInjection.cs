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
    ///     Adds the Public API v0.x OpenAPI documents to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    internal static void AddV0OpenApiDocuments(this IServiceCollection services)
    {
        services.AddOpenApi("public-api-v0.1", options =>
        {
            options.ShouldInclude = apiDescription => apiDescription.GroupName == EndpointNames.Group &&
                                                      apiDescription.GetApiVersion() is { MajorVersion: 0, MinorVersion: 1 };

            options.AddDocumentTransformer<ApiKeySecurityDocumentTransformer>()
                .AddSchemaTransformer<ExampleSchemaTransformer>()
                .AddOperationTransformer<ParameterExampleOperationTransformer>()
                .AddOperationTransformer<ProblemDetailsResponseExampleOperationTransformer>()
                .AddDocumentTransformer(new InfoDocumentTransformer("v0.1"));
        });

        services.AddOpenApi("public-api-v0.2", options =>
        {
            options.ShouldInclude = apiDescription => apiDescription.GroupName == EndpointNames.Group &&
                                                      apiDescription.GetApiVersion() is { MajorVersion: 0, MinorVersion: 2 };

            options.AddDocumentTransformer<ApiKeySecurityDocumentTransformer>()
                .AddSchemaTransformer<ExampleSchemaTransformer>()
                .AddOperationTransformer<ParameterExampleOperationTransformer>()
                .AddOperationTransformer<ProblemDetailsResponseExampleOperationTransformer>()
                .AddDocumentTransformer(new InfoDocumentTransformer("v0.2"));
        });
    }
}
