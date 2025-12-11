using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Components.Endpoints;

/// <summary>
///     Extension methods for the <see cref="IEndpointRouteBuilder" /> interface.
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    ///     Maps one or more endpoints to the endpoint route builder, as defined in the endpoint mapper type.
    /// </summary>
    /// <param name="routeBuilder">The endpoint route builder.</param>
    /// <typeparam name="TMapper">The endpoint mapper type.</typeparam>
    /// <returns>
    ///     The original <see cref="IEndpointRouteBuilder" /> instance, so that method invocations can be chained.
    /// </returns>
    public static IEndpointRouteBuilder Map<TMapper>(this IEndpointRouteBuilder routeBuilder)
        where TMapper : IEndpointMapper, new()
    {
        TMapper mapper = new();

        mapper.Map(routeBuilder);

        return routeBuilder;
    }
}
