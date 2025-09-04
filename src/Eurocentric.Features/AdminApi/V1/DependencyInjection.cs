using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.OpenApi;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AdminApi.V1;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the OpenAPI documents for the Admin API v1.x releases to the application service descriptor collection.
    /// </summary>
    /// <param name="services">The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</param>
    internal static void AddV1OpenApiDocuments(this IServiceCollection services) => services.AddOpenApi("admin-api-v1.0",
        options =>
        {
            options.ShouldInclude = description =>
                description.GroupName == Endpoints.Group
                && description.GetApiVersion() is { MajorVersion: 1, MinorVersion: 0 };

            options.AddDocumentTransformer<ApiKeySecurityDocumentTransformer>()
                .AddDocumentTransformer<V1Point0ApiReleaseDocumentTransformer>()
                .AddDocumentTransformer<V1Point0ApiInfoDocumentTransformer>()
                .AddOperationTransformer<V1ParameterExampleOperationTransformer>()
                .AddOperationTransformer<ProblemDetailsExampleOperationTransformer>()
                .AddSchemaTransformer<V1ExampleSchemaTransformer>();
        });
}
