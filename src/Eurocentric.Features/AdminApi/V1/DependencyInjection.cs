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
    ///     Adds the OpenAPI documents for version 1 of the Admin API to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddAdminApiV1OpenApiDocuments(this IServiceCollection services)
    {
        services.AddOpenApi(OpenApiDocumentNames.AdminApiV1Point0, options =>
        {
            options.ShouldInclude = description =>
                description.GroupName == ApiReleases.EndpointGroupName
                && description.GetApiVersion() is { MajorVersion: 1, MinorVersion: 0 };

            options.AddDocumentTransformer<ApiKeySecurityDocumentTransformer>()
                .AddOperationTransformer<ParameterExampleOperationTransformer>()
                .AddSchemaTransformer<ExampleSchemaTransformer>()
                .AddOperationTransformer<ProblemDetailsResponseExampleOperationTransformer>()
                .AddDocumentTransformer(new InfoDocumentTransformer("Eurocentric Admin API",
                    "A web API for modelling the Eurovision Song Contest, 2016-2025."));
        });

        return services;
    }
}
