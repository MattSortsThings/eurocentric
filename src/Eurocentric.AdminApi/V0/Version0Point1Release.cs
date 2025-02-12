using Asp.Versioning;
using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Eurocentric.AdminApi.V0;

internal static class Version0Point1Release
{
    private const string GroupName = "admin-api-v0.1";

    internal static IEndpointRouteBuilder MapVersion0Point1Release(this IEndpointRouteBuilder api)
    {
        RouteGroupBuilder versionGroup = api.MapGroup("v{version:apiVersion}")
            .HasApiVersion(new ApiVersion(0, 1))
            .WithGroupName(GroupName);

        versionGroup.MapCreateCalculationEndpoint();

        return api;
    }

    internal static IServiceCollection AddVersion0Point1OpenApiDocument(this IServiceCollection services) =>
        services.AddOpenApi(GroupName, options =>
        {
            options.AddDocumentTransformer<DocumentTransformer>()
                .AddOperationTransformer<CreateCalculationEndpoint.OperationTransformer>();
        });

    private sealed class DocumentTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document,
            OpenApiDocumentTransformerContext context,
            CancellationToken cancellationToken)
        {
            document.Info.Title = "Eurocentric Admin API";
            document.Info.Version = "v0.1";
            document.Info.Description = "An API for modelling the Eurovision Song Contest, 2016-present.";

            return Task.CompletedTask;
        }
    }
}
