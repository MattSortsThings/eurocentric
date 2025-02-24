using Asp.Versioning;
using Eurocentric.Shared.Documentation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.ApiRegistration;

/// <summary>
///     Registers an API on application startup.
/// </summary>
/// <remarks>
///     Register this type as a transient service in each API project, with an API descriptor type that is located in
///     the API project.
/// </remarks>
/// <example>
///     <code>
///     services.AddTransient&lt;IApiRegistrar,ApiRegistrar&lt;StationsApiDescriptor&gt;&gt;();
///     </code>
/// </example>
/// <typeparam name="TApi">Contains descriptive properties for the API.</typeparam>
public sealed class ApiRegistrar<TApi> : IApiRegistrar
    where TApi : class, IApiInfo, new()
{
    private readonly TApi _apiInfo = new();

    /// <inheritdoc />
    public void MapVersionedEndpoints(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.NewVersionedApi(_apiInfo.ApiId)
            .MapGroup(_apiInfo.UrlPrefix + "/v{version:apiVersion}")
            .WithGroupName(_apiInfo.EndpointGroupName)
            .RequireAuthorization(_apiInfo.AuthorizationPolicyName);

        foreach (Action<IEndpointRouteBuilder> mapper in GetEndpointMappingActions())
        {
            mapper.Invoke(api);
        }
    }

    /// <inheritdoc />
    public void AddOpenApiDocuments(IServiceCollection services)
    {
        foreach (Action<IServiceCollection> creator in GetOpenApiDocumentCreationActions())
        {
            creator.Invoke(services);
        }
    }

    private IEnumerable<Action<IEndpointRouteBuilder>> GetEndpointMappingActions()
    {
        var (apiVersions, endpoints) = ScanAssemblyForApiVersionsAndEndpoints();

        foreach (IEndpointInfo endpoint in endpoints)
        {
            yield return builder => InitializeRouteHandlerBuilder(endpoint)
                .Invoke(builder)
                .WithName(endpoint.EndpointId)
                .HasApiVersions(apiVersions.Where(version => endpoint.InitialApiVersion.ShouldIncludeIn(version)))
                .ProducesProblem(endpoint.ProblemStatusCodes)
                .WithSummary(endpoint.Summary)
                .WithDescription(endpoint.Description)
                .WithTags(endpoint.Tag);
        }
    }

    private IEnumerable<Action<IServiceCollection>> GetOpenApiDocumentCreationActions()
    {
        var (apiVersions, endpoints) = ScanAssemblyForApiVersionsAndEndpoints();

        foreach (ApiVersion apiVersion in apiVersions)
        {
            yield return services => services.AddOpenApi(CreateOpenApiDocumentName(apiVersion),
                CreateOpenApiConfigurationAction(apiVersion, endpoints));
        }
    }

    private Action<OpenApiOptions> CreateOpenApiConfigurationAction(ApiVersion apiVersion, IEndpointInfo[] endpoints)
    {
        string groupName = _apiInfo.EndpointGroupName;
        string title = _apiInfo.OpenApiDocumentTitle;
        string description = _apiInfo.OpenApiDocumentDescription;

        ApiVersion version = apiVersion;

        return options =>
        {
            options.ShouldInclude = apiDescription => apiDescription.ShouldIncludeIn(groupName, version);
            options.AddDocumentTransformer<ApiKeySecuritySchemeTransformer>();
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info.Title = title;
                document.Info.Description = description;
                document.Info.Version = $"v{version.MajorVersion}.{version.MinorVersion}";

                return Task.CompletedTask;
            });
        };
    }


    private (ApiVersion[] ApiVersions, IEndpointInfo[] Endpoints) ScanAssemblyForApiVersionsAndEndpoints()
    {
        IEndpointInfo[] endpoints = _apiInfo.GetType().Assembly.GetTypes()
            .Where(type => typeof(IEndpointInfo).IsAssignableFrom(type) && type is { IsAbstract: false, IsInterface: false })
            .OrderBy(type => type.Name)
            .Select(type => Activator.CreateInstance(type) as IEndpointInfo is var info && info is not null
                ? info
                : throw new InvalidOperationException($"Could not instantiate type {type.Name} as IEndpointInfo."))
            .ToArray();

        ApiVersion[] apiVersions = endpoints.Select(endpoint => endpoint.InitialApiVersion)
            .Distinct()
            .OrderBy(apiVersion => apiVersion)
            .ToArray();

        return (apiVersions, endpoints);
    }

    private string CreateOpenApiDocumentName(ApiVersion apiVersion) =>
        $"{_apiInfo.EndpointGroupName}-v{apiVersion.MajorVersion}.{apiVersion.MinorVersion}";


    private static Func<IEndpointRouteBuilder, RouteHandlerBuilder> InitializeRouteHandlerBuilder(IEndpointInfo endpoint)
    {
        var (resource, handler, method) = (endpoint.Resource, endpoint.Handler, endpoint.Method);

        if (method == HttpMethod.Get)
        {
            return builder => builder.MapGet(resource, handler);
        }

        if (method == HttpMethod.Post)
        {
            return builder => builder.MapPost(resource, handler);
        }

        if (method == HttpMethod.Put)
        {
            return builder => builder.MapPut(resource, handler);
        }

        if (method == HttpMethod.Delete)
        {
            return builder => builder.MapDelete(resource, handler);
        }

        throw new InvalidOperationException($"Method {method} is not supported.");
    }
}
