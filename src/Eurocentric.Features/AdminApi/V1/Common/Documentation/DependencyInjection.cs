using Eurocentric.Features.Shared.Documentation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AdminApi.V1.Common.Documentation;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    private const string EndpointGroupName = "AdminApi.V1";

    /// <summary>
    ///     Registers the OpenAPI documents for the Admin API Version 1 with the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection RegisterAdminApiV1OpenApiDocuments(this IServiceCollection services)
    {
        services.AddOpenApi("admin-api-v1.0", options =>
        {
            options.ShouldInclude = description =>
                description.GroupName == EndpointGroupName
                && description.GetApiVersion() is { MajorVersion: 1, MinorVersion: 0 };

            options.AddDocumentTransformer<ApiKeySecurityDocumentTransformer>()
                .AddOperationTransformer<ParameterExampleOperationTransformer>()
                .AddSchemaTransformer<ExampleSchemaTransformer>()
                .AddOperationTransformer<ProblemDetailsResponseExampleOperationTransformer>()
                .AddDocumentTransformer(new InfoDocumentTransformer("Eurocentric Admin API",
                    "A web API for modelling the Eurovision Song Contest, 2016-present"));
        });

        return services;
    }
}
