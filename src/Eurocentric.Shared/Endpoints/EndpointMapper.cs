using Eurocentric.Shared.ApiAbstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Shared.Endpoints;

public sealed class EndpointMapper<TApiInfo> : IEndpointMapper
    where TApiInfo : class, IApiInfo, new()
{
    private readonly TApiInfo _apiInfo = new();

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.MapGroup(_apiInfo.UrlPrefix)
            .WithGroupName(_apiInfo.EndpointGroupName);

        IEnumerable<IEndpointInfo> endpoints = _apiInfo.GetType().Assembly.GetTypes()
            .Where(type => typeof(IEndpointInfo).IsAssignableFrom(type) && type is { IsAbstract : false, IsInterface: false })
            .Select(type => (IEndpointInfo)Activator.CreateInstance(type)!);

        foreach (IEndpointInfo endpoint in endpoints)
        {
            Func<IEndpointRouteBuilder, RouteHandlerBuilder>? builder = InitializeRouteHandlerBuilder(endpoint);
            builder.Invoke(apiGroup).WithName(endpoint.Name);
        }
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
