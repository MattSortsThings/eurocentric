using Asp.Versioning;
using Eurocentric.Shared.ApiAbstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Shared.Documentation;

public sealed class OpenApiGenerator<TApiInfo> : ApiAssemblyScanner<TApiInfo>, IOpenApiGenerator
    where TApiInfo : class, IApiInfo, new()
{
    public void AddOpenApiDocuments(IServiceCollection services)
    {
        foreach (Action<IServiceCollection> action in GetOpenApiDocumentCreationActions())
        {
            action.Invoke(services);
        }
    }

    private IEnumerable<Action<IServiceCollection>> GetOpenApiDocumentCreationActions()
    {
        (ApiVersion[] apiVersions, Dictionary<Type, IOpenApiAny> examples) = GetApiVersionsAndExamples();

        foreach (ApiVersion apiVersion in apiVersions)
        {
            string documentName = CreateDocumentName(apiVersion);

            yield return services => services.AddOpenApi(documentName, options =>
            {
                options.ShouldInclude = CreateInclusionPredicate(apiVersion);
                options.AddDocumentTransformer(CreateDocumentInfoTransformer(apiVersion));
                options.AddOperationTransformer<OperationProblemDetailsResponseTransformer>();
                options.AddSchemaTransformer(new SchemaExampleTransformer(examples));
            });
        }
    }

    private string CreateDocumentName(ApiVersion apiVersion) => $"{ApiInfo.EndpointGroupName}-v{apiVersion}";

    private Func<ApiDescription, bool> CreateInclusionPredicate(ApiVersion apiVersion) => description =>
        description.GroupName == ApiInfo.EndpointGroupName && description.GetApiVersion() is { } version &&
        version == apiVersion;

    private DocumentInfoTransformer CreateDocumentInfoTransformer(ApiVersion apiVersion) =>
        new(ApiInfo.Title, ApiInfo.Description, apiVersion);

    private (ApiVersion[] ApiVersions, Dictionary<Type, IOpenApiAny> Examples) GetApiVersionsAndExamples()
    {
        (IEndpointInfo[] endpoints, ApiVersion[] apiVersions) = ScanAssemblyForEndpointsAndApiVersions();

        return (apiVersions, CreateExamples(endpoints));
    }

    private static Dictionary<Type, IOpenApiAny> CreateExamples(IEndpointInfo[] endpoints) =>
        endpoints.SelectMany(endpoint => endpoint.Examples)
            .ToDictionary(example => example.GetType(), example => example.ToOpenApiAny());
}
