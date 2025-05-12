using Eurocentric.Features.Shared.Documentation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AdminApi.V0.Common.Documentation;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    private const string EndpointGroupName = "AdminApi.V0";

    /// <summary>
    ///     Registers the OpenAPI documents for the Admin API Version 0 with the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection RegisterAdminApiV0OpenApiDocuments(this IServiceCollection services)
    {
        services.AddOpenApi("admin-api-v0.1", options =>
        {
            options.ShouldInclude = description =>
                description.GroupName == EndpointGroupName
                && description.GetApiVersion() is { MajorVersion: 0, MinorVersion: 1 };

            options.AddDocumentTransformer<ApiKeySecuritySchemeTransformer>()
                .AddDocumentTransformer<V0Point1DocumentInfoTransformer>()
                .AddOperationTransformer<V0ParameterExampleTransformer>()
                .AddSchemaTransformer<V0SchemaExampleTransformer>()
                .AddOperationTransformer<ProblemDetailsExampleTransformer>();
        });

        services.AddOpenApi("admin-api-v0.2", options =>
        {
            options.ShouldInclude = description =>
                description.GroupName == EndpointGroupName
                && description.GetApiVersion() is { MajorVersion: 0, MinorVersion: 2 };

            options.AddDocumentTransformer<ApiKeySecuritySchemeTransformer>()
                .AddDocumentTransformer<V0Point2DocumentInfoTransformer>()
                .AddOperationTransformer<V0ParameterExampleTransformer>()
                .AddSchemaTransformer<V0SchemaExampleTransformer>()
                .AddOperationTransformer<ProblemDetailsExampleTransformer>();
        });

        return services;
    }
}
