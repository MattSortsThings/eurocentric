using Asp.Versioning;
using Eurocentric.PublicApi.V0.Greetings.GetGreetings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Eurocentric.PublicApi.V0;

internal static class Version0Point1Release
{
    private const string GroupName = "public-api-v0.1";

    internal static IEndpointRouteBuilder MapVersion0Point1Release(this IEndpointRouteBuilder api)
    {
        RouteGroupBuilder versionGroup = api.MapGroup("v{version:apiVersion}")
            .HasApiVersion(new ApiVersion(0, 1))
            .WithGroupName(GroupName);

        versionGroup.MapGetGreetings();

        return api;
    }

    internal static IServiceCollection AddVersion0Point1OpenApiDocument(this IServiceCollection services) =>
        services.AddOpenApi(GroupName, options =>
        {
            options.AddDocumentTransformer<DocumentTransformer>()
                .AddOperationTransformer<GetGreetingsEndpoint.OperationTransformer>();
        });

    private sealed class DocumentTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document,
            OpenApiDocumentTransformerContext context,
            CancellationToken cancellationToken)
        {
            document.Info.Title = "Eurocentric Public API";
            document.Info.Version = "v0.1";
            document.Info.Description = "An API for (over)analysing the Eurovision Song Contest, 2016-present.";

            return Task.CompletedTask;
        }
    }
}
