using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Common.OpenApi;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.PublicApi.V1;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the Public API v1.x OpenAPI documents to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    internal static void AddV1OpenApiDocuments(this IServiceCollection services) =>
        services.AddOpenApi("public-api-v1.0", ConfigurePublicApiV1Point0);

    private static void ConfigurePublicApiV1Point0(OpenApiOptions options)
    {
        options.ShouldInclude = apiDescription => apiDescription.GroupName == EndpointNames.Group &&
                                                  apiDescription.GetApiVersion() is { MajorVersion: 1, MinorVersion: 0 };

        options.AddDocumentTransformer<ApiKeySecurityDocumentTransformer>()
            .AddSchemaTransformer<ExampleSchemaTransformer>()
            .AddOperationTransformer<ParameterExampleOperationTransformer>()
            .AddOperationTransformer<ProblemDetailsResponseExampleOperationTransformer>()
            .AddDocumentTransformer(new InfoDocumentTransformer("v1.0"))
            .AddDocumentTransformer<TagsDocumentTransformer>();
    }
}
