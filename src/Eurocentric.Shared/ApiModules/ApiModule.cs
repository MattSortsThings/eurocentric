using Asp.Versioning;
using Eurocentric.Shared.Documentation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.ApiModules;

/// <summary>
///     Abstract base class for an API module.
/// </summary>
/// <remarks>Define a concrete derivative of this abstract base class in each API project.</remarks>
public abstract class ApiModule : IApiEndpointsMapper, IApiDocumentsRegistrar
{
    /// <summary>
    ///     Gets the unique name of the API.
    /// </summary>
    /// <example>
    ///     <c>"CustomersApi"</c>
    /// </example>
    protected abstract string ApiName { get; }

    /// <summary>
    ///     Gets the unique URL prefix for the API, which must include a version segment.
    /// </summary>
    /// <example>
    ///     <c>"customers/api/v{version:ApiVersion}"</c>
    /// </example>
    protected abstract string UrlPrefix { get; }

    /// <summary>
    ///     Gets the unique endpoint group name for the API, which is used to generate documentation URLs.
    /// </summary>
    /// <example>
    ///     <c>"customers-api"</c>
    /// </example>
    protected abstract string EndpointGroupName { get; }

    /// <summary>
    ///     Gets the authorization policy name for the API.
    /// </summary>
    /// <example>
    ///     <c>"CustomersApiAuthorizationPolicy".</c>
    /// </example>
    protected abstract string? AuthorizationPolicyName { get; }

    protected abstract string OpenApiDocumentTitle { get; }

    protected abstract string OpenApiDocumentDescription { get; }

    public void AddOpenApiDocuments(IServiceCollection services)
    {
        foreach (var (docName, configureAction) in GetOpenApiDocumentData())
        {
            services.AddOpenApi(docName, configureAction);
        }
    }

    public void Map(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi(ApiName)
            .MapGroup(UrlPrefix)
            .WithGroupName(EndpointGroupName);

        foreach (Action<IEndpointRouteBuilder> mapper in GetEndpointMappers())
        {
            mapper(apiGroup);
        }

        if (AuthorizationPolicyName is not null)
        {
            apiGroup.RequireAuthorization(AuthorizationPolicyName);
        }
    }

    private IEnumerable<Action<IEndpointRouteBuilder>> GetEndpointMappers()
    {
        (ApiVersion[] apiVersions, IApiEndpoint[] endpoints) = ScanAssemblyForVersionedApiEndpointTypes();

        foreach (IApiEndpoint endpoint in endpoints)
        {
            yield return builder => endpoint.Map(builder)
                .WithName(endpoint.EndpointName)
                .WithApiVersions(apiVersions.Where(version => endpoint.InitialApiVersion.IsIncludedIn(version)));
        }
    }

    private IEnumerable<OpenApiDocumentDatum> GetOpenApiDocumentData()
    {
        (ApiVersion[] apiVersions, IApiEndpoint[] endpoints) = ScanAssemblyForVersionedApiEndpointTypes();

        foreach (ApiVersion apiVersion in apiVersions)
        {
            string docName = $"{EndpointGroupName}-v{apiVersion.MajorVersion}.{apiVersion.MinorVersion}";

            Action<OpenApiOptions> configureAction = options =>
            {
                options.ShouldInclude = description => description.Matches(EndpointGroupName, apiVersion);
                options.AddDocumentTransformer<ApiKeySecuritySchemeTransformer>();
                options.AddDocumentTransformer((document, _, _) =>
                {
                    document.Info.Title = OpenApiDocumentTitle;
                    document.Info.Description = OpenApiDocumentDescription;
                    document.Info.Version = $"v{apiVersion.MajorVersion}.{apiVersion.MinorVersion}";

                    return Task.CompletedTask;
                });
            };

            configureAction = endpoints
                .Where(apiEndpoint => apiEndpoint.InitialApiVersion.MajorVersion == apiVersion.MajorVersion &&
                                      apiEndpoint.InitialApiVersion.MinorVersion <= apiVersion.MinorVersion)
                .Aggregate(configureAction, (current, apiEndpoint) => current + apiEndpoint.Configure);

            yield return new OpenApiDocumentDatum(docName, configureAction);
        }
    }

    private (ApiVersion[], IApiEndpoint[]) ScanAssemblyForVersionedApiEndpointTypes()
    {
        IApiEndpoint[] endpoints = GetType().Assembly.GetTypes()
            .Where(type => typeof(IApiEndpoint).IsAssignableFrom(type) && type is { IsAbstract: false, IsInterface: false })
            .Select(type => (IApiEndpoint)Activator.CreateInstance(type)!)
            .ToArray();

        ApiVersion[] apiVersions = endpoints.Select(endpoint => endpoint.InitialApiVersion)
            .Distinct()
            .OrderBy(version => version)
            .ToArray();

        return (apiVersions, endpoints);
    }

    private readonly record struct OpenApiDocumentDatum(string DocumentName, Action<OpenApiOptions> ConfigureAction);
}
