using Asp.Versioning;
using Eurocentric.Shared.ApiAbstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Shared.Endpoints;

public sealed class EndpointMapper<TApiInfo> : ApiAssemblyScanner<TApiInfo>, IEndpointMapper
    where TApiInfo : class, IApiInfo, new()
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.NewVersionedApi(ApiInfo.Id)
            .MapGroup(ApiInfo.UrlPrefix)
            .WithGroupName(ApiInfo.EndpointGroupName)
            .RequireAuthorization(ApiInfo.AuthorizationPolicyName)
            .ProducesProblems(ApiInfo.ProblemStatusCodes);

        foreach (Action<IEndpointRouteBuilder> mapper in GetEndpointMappingActions())
        {
            mapper.Invoke(api);
        }

        Console.WriteLine(api);
    }

    private IEnumerable<Action<IEndpointRouteBuilder>> GetEndpointMappingActions()
    {
        var (endpoints, versions) = ScanAssemblyForEndpointsAndApiVersions();

        foreach (IEndpointInfo endpoint in endpoints)
        {
            yield return builder => InitializeRouteHandlerBuilder(endpoint)
                .Invoke(builder)
                .WithName(endpoint.Name)
                .WithSummary(endpoint.Summary)
                .WithDescription(endpoint.Description)
                .ProducesProblems(endpoint.ProblemStatusCodes)
                .HasApiVersions(GetApplicableApiVersions(endpoint, versions));
        }
    }

    private static IEnumerable<ApiVersion> GetApplicableApiVersions(IEndpointInfo endpoint, ApiVersion[] apiVersions)
    {
        (int major, int minor) = (endpoint.MajorApiVersion, endpoint.MinorApiVersion);

        return apiVersions.Where(version => version.MajorVersion == major && version.MinorVersion >= minor);
    }

    private static Func<IEndpointRouteBuilder, RouteHandlerBuilder> InitializeRouteHandlerBuilder(IEndpointInfo endpoint)
    {
        var (resource, handler, method) = (endpoint.Route, endpoint.Handler, endpoint.Method);

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
